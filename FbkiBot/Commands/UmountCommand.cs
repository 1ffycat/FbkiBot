using FbkiBot.Attributes;
using FbkiBot.Data;
using FbkiBot.Models;
using FbkiBot.Resources;
using FbkiBot.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace FbkiBot.Commands;

[BotCommand("/umount", "Отвязывает сохранения из этого чата от личной переписки с ботом")]
public class UmountCommand(BotDbContext db, ILogger<UmountCommand> logger) : IChatCommand
{
    public bool CanExecute(CommandContext context)
    {
        return context.Command?.Equals("/umount", StringComparison.OrdinalIgnoreCase) ?? false;
    }

    public async Task ExecuteAsync(ITelegramBotClient botClient, CommandContext context,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Processing umount command");

        UserMount? mount;
        // Если задано название монтирования - ищем по нему и ID пользователя
        if (context.Argument?.Length > 0)
            mount = await db.UserMounts.FirstOrDefaultAsync(mnt =>
                mnt.UserId == context.Message.From!.Id &&
                string.Equals(mnt.Name, context.Argument, StringComparison.OrdinalIgnoreCase), cancellationToken);
        else // Если не задано - ищем по ID пользователя и ID текущего чата
            mount = await db.UserMounts.FirstOrDefaultAsync(mnt =>
                mnt.UserId == context.Message.From!.Id && mnt.ChatId == context.Message.Chat.Id, cancellationToken);

        // Если такое монтирование не найдено - сообщаем об этом пользователю 
        if (mount is null)
        {
            await botClient.TrySendMessageOrNotify(context.Message.From!, CommandStrings.Umount_NotFound,
                context.Message.Chat,
                cancellationToken);
            await botClient.DeleteMessage(context.Message.Chat, context.Message.Id, cancellationToken);
            return;
        }

        // Удаляем монтирование и сохраняем изменения в БД
        db.UserMounts.Remove(mount);
        await db.SaveChangesAsync(cancellationToken);

        logger.LogDebug("/umount - success");
        await botClient.TrySendMessageOrNotify(context.Message.From!, CommandStrings.Umount_Success,
            context.Message.Chat,
            cancellationToken);
        await botClient.DeleteMessage(context.Message.Chat, context.Message.Id, cancellationToken);
    }
}