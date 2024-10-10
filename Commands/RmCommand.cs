using FbkiBot.Attributes;
using FbkiBot.Configuration;
using FbkiBot.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FbkiBot.Commands;

[BotCommand("/rm", "Удаляет сохраненное сообщение", "/rm <название>")]
public class RmCommand(IOptions<MessageSavingSettings> saveSettings, IOptions<TextConstSettings> textConsts, ILogger<RmCommand> logger, BotDbContext db) : IChatCommand
{
    public bool CanExecute(Message message) => message.Text!.StartsWith("/rm", StringComparison.OrdinalIgnoreCase);

    public async Task ExecuteAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        logger.LogDebug("Processing rm");

        var name = IChatCommand.GetArg(message);  // Получаем текст после команды

        // Если искомое имя не задано
        if (name is null)
        {
            logger.LogDebug("/rm denied - no name provided");
            await botClient.SendTextMessageAsync(message.Chat.Id, textConsts.Value.SaveNoNameProvidedMessage, cancellationToken: cancellationToken);
            return;
        }

        var messageFound = await db.FindSavedMessageAsync(name, message.Chat.Id, cancellationToken: cancellationToken);

        // Если сообщение с таким названием не найдено
        if (messageFound is null)
        {
            logger.LogDebug("/rm denied - no message found with such name");
            await botClient.SendTextMessageAsync(message.Chat.Id, textConsts.Value.CatNotFoundMessage, cancellationToken: cancellationToken);
            return;
        }

        // Если удалять сообщения может только автор и пользователь не автор
        if (saveSettings.Value.CanOnlyBeRemovedByAuthor && messageFound.AddedById != message.From!.Id)
        {
            logger.LogDebug("/rm denied - not an author");
            await botClient.SendTextMessageAsync(message.Chat.Id, textConsts.Value.RmNotAuthorMessage, replyToMessageId: messageFound.MessageId, cancellationToken: cancellationToken);
        }

        // Удаляем сообщение и сохраняем изменения в БД.
        db.SavedMessages.Remove(messageFound);
        await db.SaveChangesAsync(cancellationToken: cancellationToken);

        logger.LogDebug("Saved message removed");

        await botClient.SendTextMessageAsync(message.Chat.Id, textConsts.Value.RmSuccess, cancellationToken: cancellationToken);
    }
}