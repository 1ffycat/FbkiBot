using FbkiBot.Attributes;
using FbkiBot.Configuration;
using FbkiBot.Data;
using FbkiBot.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FbkiBot.Commands;

[BotCommand("/cat", "Находит сохраненное сообщение по названию", "/cat <название>")]
public class CatCommand(IOptions<TextConstSettings> textConsts, BotDbContext db, ILogger<CatCommand> logger) : IChatCommand
{
    public bool CanExecute(CommandContext context) => context.Command?.Equals("/cat", StringComparison.OrdinalIgnoreCase) ?? false;

    public async Task ExecuteAsync(ITelegramBotClient botClient, CommandContext context, CancellationToken cancellationToken)
    {
        logger.LogDebug("Processing cat");

        // Если название искомого сообщение не задано
        if (context.Argument is null)
        {
            logger.LogDebug("/cat denied - no name provided");
            await botClient.SendTextMessageAsync(context.Message.Chat.Id, textConsts.Value.SaveNoNameProvidedMessage, cancellationToken: cancellationToken);
            return;
        }

        // Ищем сообщение по ID чата и названию
        var messageFound = await db.FindSavedMessageAsync(context.Argument, context.Message.Chat.Id, cancellationToken);

        // Если такого сообщения не найдено
        if (messageFound is null)
        {
            logger.LogDebug("/cat denied - no message found by name");
            await botClient.SendTextMessageAsync(context.Message.Chat.Id, textConsts.Value.CatNotFoundMessage, cancellationToken: cancellationToken);
            return;
        }

        logger.LogDebug("/cat - success");

        await botClient.SendTextMessageAsync(context.Message.Chat.Id, textConsts.Value.CatFoundMessage, replyToMessageId: messageFound.MessageId, cancellationToken: cancellationToken);
    }
}