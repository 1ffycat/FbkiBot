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

[BotCommand("/rmmount", "Удаляет mount", "/mount <название>")]
public class RmMountCommand(BotDbContext db, ILogger<RmMountCommand> logger, IOptions<TextConstSettings> textConsts) : IChatCommand
{
    public bool CanExecute(CommandContext context) => context.Command?.Equals("/rmmount", StringComparison.OrdinalIgnoreCase) ?? false;

    public async Task ExecuteAsync(ITelegramBotClient botClient, CommandContext context, CancellationToken cancellationToken)
    {
        logger.LogDebug("Processing rmmount command");

        // Если нет mount с этим чатом
        if (!db.UserMounts.Any(mnt => mnt.UserId == context.Message.From!.Id && mnt.ChatId == context.Message.Chat.Id))
        {
            await botClient.SendTextMessageAsync(context.Message.Chat.Id, textConsts.Value.RmMountNotFoundMessage, cancellationToken: cancellationToken);
            return;
        }

        db.UserMounts.Remove(await db.UserMounts.FirstAsync(mnt => mnt.UserId == context.Message.From!.Id && mnt.ChatId == context.Message.Chat.Id));
        await db.SaveChangesAsync(cancellationToken: cancellationToken);

        logger.LogDebug("/rmmount - success");
        await botClient.SendTextMessageAsync(context.Message.Chat.Id, textConsts.Value.RmSuccessMessage, cancellationToken: cancellationToken);
    }
}