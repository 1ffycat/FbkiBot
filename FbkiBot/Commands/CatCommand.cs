using FbkiBot.Attributes;
using FbkiBot.Data;
using FbkiBot.Models;
using FbkiBot.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace FbkiBot.Commands;

[BotCommand("/cat", "Находит сохраненное сообщение по названию", "/cat <название>")]
public class CatCommand(BotDbContext db, ILogger<CatCommand> logger) : IChatCommand
{
    public bool CanExecute(CommandContext context)
    {
        return context.Command?.Equals("/cat", StringComparison.OrdinalIgnoreCase) ?? false;
    }

    public async Task ExecuteAsync(ITelegramBotClient botClient, CommandContext context,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Processing cat");

        // Если название искомого сообщение не задано
        if (context.Argument is null)
        {
            logger.LogDebug("/cat denied - no name provided");
            await botClient.SendMessage(context.Message.Chat.Id, CommandStrings.NoSavedMessageNameProvided,
                cancellationToken: cancellationToken);
            return;
        }

        SavedMessage? messageFound;
        var mountName = "";
        var indexSlash = context.Argument.IndexOf('/');
        // Если в сообщении есть / - получаем имя примонтированого чата пользователя
        if (indexSlash != -1)
            mountName = context.Argument[..indexSlash]; // Весь текст до первого слеша

        // Ищем сообщение по ID чата или по ID примонтированного чата, если он задан, и названию
        if (await db.UserMounts.SingleOrDefaultAsync(
                mnt => mnt.UserId == context.Message.From!.Id && EF.Functions.Like(mnt.Name, mountName),
                cancellationToken) is { } mount)
            messageFound =
                await db.FindSavedMessageAsync(context.Argument[(indexSlash + 1)..], mount.ChatId, cancellationToken);
        else
            messageFound = await db.FindSavedMessageAsync(context.Argument, context.Message.Chat.Id, cancellationToken);

        // Если такого сообщения не найдено
        if (messageFound is null)
        {
            logger.LogDebug("/cat denied - no message found by name");
            await botClient.SendMessage(context.Message.Chat.Id, CommandStrings.SavedMessageNotFound,
                cancellationToken: cancellationToken);
            return;
        }

        logger.LogDebug("/cat - success");

        await botClient.ForwardMessage(context.Message.Chat.Id, messageFound.ChatId, messageFound.MessageId,
            cancellationToken: cancellationToken);
    }
}