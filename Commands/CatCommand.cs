using FbkiBot.Attributes;
using FbkiBot.Configuration;
using FbkiBot.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FbkiBot.Commands;

[BotCommand("/cat", "Находит сохраненное сообщение по названию", "/cat <название>")]
public class CatCommand(IOptions<TextConstSettings> textConsts, BotDbContext db, ILogger<CatCommand> logger) : IChatCommand
{
    public bool CanExecute(Message message) => message.Text!.StartsWith("/cat", StringComparison.OrdinalIgnoreCase);

    public async Task ExecuteAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        logger.LogDebug("Processing cat");

        var arg = IChatCommand.GetArg(message);  // Получаем текст после команды

        // Если название искомого сообщение не задано
        if (arg is null)
        {
            logger.LogDebug("/cat denied - no name provided");
            await botClient.SendTextMessageAsync(message.Chat.Id, textConsts.Value.SaveNoNameProvidedMessage, cancellationToken: cancellationToken);
            return;
        }

        // Ищем сообщение по ID чата и названию
        var messageFound = await db.FindSavedMessage(arg, message.Chat.Id, cancellationToken);

        // Если такого сообщения не найдено
        if (messageFound is null)
        {
            logger.LogDebug("/cat denied - no message found by name");
            await botClient.SendTextMessageAsync(message.Chat.Id, textConsts.Value.CatNotFoundMessage, cancellationToken: cancellationToken);
            return;
        }

        logger.LogDebug("/cat - success");

        await botClient.SendTextMessageAsync(message.Chat.Id, textConsts.Value.CatFoundMessage, replyToMessageId: messageFound.MessageId, cancellationToken: cancellationToken);
    }
}