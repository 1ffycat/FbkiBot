using System.Security.Cryptography.X509Certificates;
using FbkiBot.Attributes;
using FbkiBot.Configuration;
using FbkiBot.Data;
using FbkiBot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FbkiBot.Commands;

[BotCommand("/umount", "Удаляет mount", "/umount")]
public class UmountCommand(BotDbContext db, ILogger<UmountCommand> logger, IOptions<TextConstSettings> textConsts) : IChatCommand
{
    public bool CanExecute(CommandContext context) => context.Command?.Equals("/umount", StringComparison.OrdinalIgnoreCase) ?? false;

    public async Task ExecuteAsync(ITelegramBotClient botClient, CommandContext context, CancellationToken cancellationToken)
    {
        logger.LogDebug("Processing umount command");

        // Если нет mount с этим чатом
        if (!db.UserMounts.Any(mnt => mnt.UserId == context.Message.From!.Id && mnt.ChatId == context.Message.Chat.Id))
        {
            await botClient.SendTextMessageAsync(context.Message.Chat.Id, textConsts.Value.RmMountNotFoundMessage, cancellationToken: cancellationToken);
            return;
        }

        // Удаляем монтирование и сохраняем изменения в БД
        db.UserMounts.Remove(await db.UserMounts.FirstAsync(mnt => mnt.UserId == context.Message.From!.Id && mnt.ChatId == context.Message.Chat.Id));
        await db.SaveChangesAsync(cancellationToken: cancellationToken);

        logger.LogDebug("/umount - success");
        await botClient.SendTextMessageAsync(context.Message.Chat.Id, textConsts.Value.RmSuccessMessage, cancellationToken: cancellationToken);
    }
}