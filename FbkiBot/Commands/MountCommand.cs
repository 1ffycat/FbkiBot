using FbkiBot.Attributes;
using FbkiBot.Data;
using FbkiBot.Models;
using FbkiBot.Resources;
using FbkiBot.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FbkiBot.Commands;

[BotCommand("/mount", "Связывает сохранения из чата с личной перепиской с ботом", "/mount <название>")]
public class MountCommand(BotDbContext db, ILogger<MountCommand> logger) : IChatCommand
{
    public bool CanExecute(CommandContext context)
    {
        return context.Command?.Equals("/mount", StringComparison.OrdinalIgnoreCase) ?? false;
    }

    public async Task ExecuteAsync(ITelegramBotClient botClient, CommandContext context,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Processing mount command");

        // Если не группа
        if (context.Message.Chat.Type != ChatType.Group && context.Message.Chat.Type != ChatType.Supergroup)
        {
            logger.LogDebug("/mount denied - not a group chat");
            await botClient.SendMessage(context.Message.Chat.Id, CommandStrings.Mount_NotAGroup,
                cancellationToken: cancellationToken);
            return;
        }

        // Если не задано название монтирования
        if (context.Argument is null)
        {
            logger.LogDebug("/mount denied - no context.Argument");
            await botClient.TrySendMessageOrNotify(context.Message.From!, CommandStrings.Mount_NoName,
                context.Message.Chat,
                cancellationToken);
            await botClient.DeleteMessage(context.Message.Chat.Id, context.Message.Id, cancellationToken);
            return;
        }

        // Если монтирование с таким названием уже существует
        if (db.UserMounts.Any(mnt =>
                EF.Functions.Like(mnt.Name, context.Argument) && context.Message.From!.Id == mnt.UserId))
        {
            await botClient.TrySendMessageOrNotify(context.Message.From!, CommandStrings.Mount_NameTaken,
                context.Message.Chat,
                cancellationToken);
            await botClient.DeleteMessage(context.Message.Chat.Id, context.Message.Id, cancellationToken);
            return;
        }

        // Если монтирование для этого чата уже существует
        if (db.UserMounts.Any(mnt => mnt.UserId == context.Message.From!.Id && mnt.ChatId == context.Message.Chat.Id))
        {
            await botClient.TrySendMessageOrNotify(context.Message.From!, CommandStrings.Mount_AlreadyExists,
                context.Message.Chat,
                cancellationToken);
            await botClient.DeleteMessage(context.Message.Chat.Id, context.Message.Id, cancellationToken);
            return;
        }

        var userMount = new UserMount(context.Argument, context.Message.Chat.Id, context.Message.From!);

        logger.LogDebug("Saving UserMount to db...");

        await db.UserMounts.AddAsync(userMount, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        logger.LogDebug("/mount - success");
        await botClient.TrySendMessageOrNotify(context.Message.From!, CommandStrings.Mount_Success,
            context.Message.Chat,
            cancellationToken);
        await botClient.DeleteMessage(context.Message.Chat.Id, context.Message.Id, cancellationToken);
    }
}