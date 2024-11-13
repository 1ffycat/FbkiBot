using FbkiBot.Attributes;
using FbkiBot.Configuration;
using FbkiBot.Data;
using FbkiBot.Models;
using FbkiBot.Resources;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace FbkiBot.Commands;

[BotCommand("/umount", "Отвязывает сохранения из этого чата от личной переписки с ботом")]
public class UmountCommand(BotDbContext db, ILogger<UmountCommand> logger) : IChatCommand
{
    public bool CanExecute(CommandContext context) => context.Command?.Equals("/umount", StringComparison.OrdinalIgnoreCase) ?? false;

    public async Task ExecuteAsync(ITelegramBotClient botClient, CommandContext context, CancellationToken cancellationToken)
    {
        logger.LogDebug("Processing umount command");

        // Ищем монтирование чата по пользователю, который написал команду и Id чата в котором была написана команда
        var mount = db.UserMounts.FirstOrDefault(mnt => mnt.UserId == context.Message.From!.Id && mnt.ChatId == context.Message.Chat.Id);

        // Если такое монтирование не найдено - сообщаем об этом пользователю 
        if (mount is null)
        {
            await botClient.SendTextMessageAsync(context.Message.Chat.Id, CommandStrings.Umount_NotFound, cancellationToken: cancellationToken);
            return;
        }

        // Удаляем монтирование и сохраняем изменения в БД
        db.UserMounts.Remove(mount);
        await db.SaveChangesAsync(cancellationToken: cancellationToken);

        logger.LogDebug("/umount - success");
        await botClient.SendTextMessageAsync(context.Message.Chat.Id, CommandStrings.Umount_Success, cancellationToken: cancellationToken);
    }
}