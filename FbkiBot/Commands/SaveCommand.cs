using FbkiBot.Attributes;
using FbkiBot.Data;
using FbkiBot.Models;
using FbkiBot.Resources;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FbkiBot.Commands;

[BotCommand("/save", "Сохраняет отмеченное сообщение в БД с заданным названием", "/save <название>")]
public class SaveCommand(BotDbContext db, ILogger<SaveCommand> logger) : IChatCommand
{
    public bool CanExecute(CommandContext context)
    {
        return context.Command?.Equals("/save", StringComparison.OrdinalIgnoreCase) ?? false;
    }

    public async Task ExecuteAsync(ITelegramBotClient botClient, CommandContext context,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Processing save message command");

        // Если не задано название
        if (context.Argument is null)
        {
            logger.LogDebug("/save denied - no save name");
            await botClient.SendMessage(context.Message.Chat.Id, CommandStrings.NoSavedMessageNameProvided,
                cancellationToken: cancellationToken);
            return;
        }

        // Если не выбрано сообщение
        if (context.Message.ReplyToMessage is not { } replyMsg)
        {
            logger.LogDebug("/save denied - no reply message");
            await botClient.SendMessage(context.Message.Chat.Id, CommandStrings.Save_NoReply,
                cancellationToken: cancellationToken);
            return;
        }

        // Если сообщение с таким названием уже существует
        if (await db.FindSavedMessageAsync(context.Argument, context.Message.Chat.Id, cancellationToken) is
            { } existingMessage)
        {
            await botClient.SendMessage(context.Message.Chat.Id, CommandStrings.Save_NameTaken,
                replyParameters: new ReplyParameters()
                    { ChatId = existingMessage.ChatId, MessageId = existingMessage.MessageId },
                cancellationToken: cancellationToken);
            return;
        }

        var saveMessage = new SavedMessage(context.Argument, replyMsg.MessageId, context.Message.Chat.Id,
            context.Message.From!);

        logger.LogDebug("Saving message to db...");

        await db.SavedMessages.AddAsync(saveMessage, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        logger.LogDebug("/save - success");

        await botClient.SendMessage(context.Message.Chat.Id, CommandStrings.Save_Success,
            cancellationToken: cancellationToken);
    }
}