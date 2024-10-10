using FbkiBot.Attributes;
using FbkiBot.Configuration;
using FbkiBot.Data;
using FbkiBot.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FbkiBot.Commands;

[BotCommand("/save", "Сохраняет отмеченное сообщение в БД с заданным названием", "/save <название>")]
public class SaveCommand(BotDbContext db, ILogger<SaveCommand> logger, IOptions<TextConstSettings> textConsts) : IChatCommand
{
    public bool CanExecute(CommandContext context) => context.Command?.Equals("/save", StringComparison.OrdinalIgnoreCase) ?? false;

    public async Task ExecuteAsync(ITelegramBotClient botClient, CommandContext context, CancellationToken cancellationToken)
    {
        logger.LogDebug("Processing save message command");

        // Если не задано название
        if (context.Argument is null)
        {
            logger.LogDebug("/save denied - no save name");
            await botClient.SendTextMessageAsync(context.Message.Chat.Id, textConsts.Value.SaveNoNameProvidedMessage, cancellationToken: cancellationToken);
            return;
        }

        // Если не выбрано сообщение
        if (context.Message.ReplyToMessage is not Message replyMsg)
        {
            logger.LogDebug("/save denied - no reply message");
            await botClient.SendTextMessageAsync(context.Message.Chat.Id, textConsts.Value.SaveNoReplyMessage, cancellationToken: cancellationToken);
            return;
        }

        // Если сообщение с таким названием уже существует
        if (await db.FindSavedMessageAsync(context.Argument, context.Message.Chat.Id, cancellationToken) is SavedMessage existingMessage)
        {
            await botClient.SendTextMessageAsync(context.Message.Chat.Id, textConsts.Value.SaveNameTakenMessage, replyToMessageId: existingMessage.MessageId, cancellationToken: cancellationToken);
            return;
        }

        var saveMessage = new SavedMessage(context.Argument, replyMsg.MessageId, context.Message.Chat.Id, context.Message.From!);

        logger.LogDebug("Saving message to db...");

        await db.SavedMessages.AddAsync(saveMessage, cancellationToken: cancellationToken);
        await db.SaveChangesAsync(cancellationToken: cancellationToken);

        logger.LogDebug("/save - success");

        await botClient.SendTextMessageAsync(context.Message.Chat.Id, textConsts.Value.SaveMessageSavedMessage, cancellationToken: cancellationToken);
    }
}