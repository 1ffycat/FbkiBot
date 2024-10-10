using FbkiBot.Attributes;
using FbkiBot.Configuration;
using FbkiBot.Data;
using FbkiBot.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FbkiBot.Commands;

[BotCommand("/rm", "Удаляет сохраненное сообщение", "/rm <название>")]
public class RmCommand(IOptions<MessageSavingSettings> saveSettings, IOptions<TextConstSettings> textConsts, ILogger<RmCommand> logger, BotDbContext db) : IChatCommand
{
    public bool CanExecute(CommandContext context) => context.Command?.Equals("/rm", StringComparison.OrdinalIgnoreCase) ?? false;

    public async Task ExecuteAsync(ITelegramBotClient botClient, CommandContext context, CancellationToken cancellationToken)
    {
        logger.LogDebug("Processing rm");

        // Если искомое имя не задано
        if (context.Argument is null)
        {
            logger.LogDebug("/rm denied - no name provided");
            await botClient.SendTextMessageAsync(context.Message.Chat.Id, textConsts.Value.SaveNoNameProvidedMessage, cancellationToken: cancellationToken);
            return;
        }

        var messageFound = await db.FindSavedMessageAsync(context.Argument, context.Message.Chat.Id, cancellationToken: cancellationToken);

        // Если сообщение с таким названием не найдено
        if (messageFound is null)
        {
            logger.LogDebug("/rm denied - no message found with such name");
            await botClient.SendTextMessageAsync(context.Message.Chat.Id, textConsts.Value.CatNotFoundMessage, cancellationToken: cancellationToken);
            return;
        }

        // Если удалять сообщения может только автор и пользователь не автор
        if (saveSettings.Value.CanOnlyBeRemovedByAuthor && messageFound.AddedById != context.Message.From?.Id)
        {
            logger.LogDebug("/rm denied - not an author");
            await botClient.SendTextMessageAsync(context.Message.Chat.Id, textConsts.Value.RmNotAuthorMessage, replyToMessageId: messageFound.MessageId, cancellationToken: cancellationToken);
        }

        // Удаляем сообщение и сохраняем изменения в БД.
        db.SavedMessages.Remove(messageFound);
        await db.SaveChangesAsync(cancellationToken: cancellationToken);

        logger.LogDebug("Saved message removed");

        await botClient.SendTextMessageAsync(context.Message.Chat.Id, textConsts.Value.RmSuccessMessage, cancellationToken: cancellationToken);
    }
}