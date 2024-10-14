using System.Text;
using FbkiBot.Attributes;
using FbkiBot.Configuration;
using FbkiBot.Data;
using FbkiBot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace FbkiBot.Commands;

[BotCommand("/mounts", "Показывает список всех примонтированных чатов")]
public class MountsCommand(BotDbContext db, ILogger<MountsCommand> logger, IOptions<TextConstSettings> textConsts) : IChatCommand
{
    public bool CanExecute(CommandContext context) => context.Command?.Equals("/mounts", StringComparison.OrdinalIgnoreCase) ?? false;

    public async Task ExecuteAsync(ITelegramBotClient botClient, CommandContext context, CancellationToken cancellationToken)
    {
        logger.LogDebug("Processing mounts");

        // Ищем все монтирования для текущего пользователя
        var mounts = await db.UserMounts.Where(m => m.UserId == context.Message.From!.Id).ToListAsync(cancellationToken);

        // Если у пользователя нет ни одного монтирования
        if (mounts.Count == 0)
        {
            logger.LogDebug("Found no mounts for user");

            await botClient.SendTextMessageAsync(context.Message.Chat.Id, textConsts.Value.MountsNoMounts, cancellationToken: cancellationToken);
            return;
        }

        logger.LogDebug("Found {count} mounts for user", mounts.Count);

        // Собираем все найденные монтирования в красивый вывод:
        var sb = new StringBuilder(textConsts.Value.MountsHeader);
        sb.AppendLine();
        sb.AppendLine();

        foreach (var mount in mounts)
        {
            // Получаем информацию о примонтированном чате
            var chat = await botClient.GetChatAsync(mount.ChatId, cancellationToken: cancellationToken);

            // Если не можем получить информацию о чате (например, бота удалили из него) - пишем ID чата
            sb.AppendLine($"- {mount.Name}: {chat.Title ?? $"Chat ID: {mount.ChatId}"}");
        }

        await botClient.SendTextMessageAsync(context.Message.Chat.Id, sb.ToString(), cancellationToken: cancellationToken);

        logger.LogDebug("/mount - success");
    }
}