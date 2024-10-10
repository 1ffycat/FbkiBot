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
public class RMMountCommand(BotDbContext db, ILogger<SaveCommand> logger, IOptions<TextConstSettings> textConsts) : IChatCommand
{
    public bool CanExecute(Message message) => message.Text!.StartsWith("/rmmount", StringComparison.OrdinalIgnoreCase);

    public async Task ExecuteAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        logger.LogDebug("Processing mount command");
        var name = IChatCommand.GetArg(message);  // Получаем текст после команды

        // Если нет mount с этим чатом
        if (!db.UserMounts.Any(mnt => mnt.UserId == message.From!.Id && mnt.ChatId == message.Chat.Id))
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, "mount с этим чатом нет", cancellationToken: cancellationToken);
            return;
        }

        db.UserMounts.Remove(await db.UserMounts.FirstAsync(mnt => mnt.UserId == message.From!.Id && mnt.ChatId == message.Chat.Id));
        await db.SaveChangesAsync(cancellationToken: cancellationToken);

        logger.LogDebug("/rmmount - success");
        await botClient.SendTextMessageAsync(message.Chat.Id, "mount Успешно добавлен", cancellationToken: cancellationToken);
    }
}