using System.Text;
using FbkiBot.Attributes;
using FbkiBot.Configuration;
using FbkiBot.Data;
using FbkiBot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FbkiBot.Commands;

[BotCommand("/ls", "Показывает список сохраненных сообщений в категории", "/ls <категория>")]
public class LsCommand(ILogger<LsCommand> logger, BotDbContext db, IOptions<TextConstSettings> textConsts) : IChatCommand
{
    public bool CanExecute(CommandContext context) => context.Command?.Equals("/ls", StringComparison.OrdinalIgnoreCase) ?? false;

    public async Task ExecuteAsync(ITelegramBotClient botClient, CommandContext context, CancellationToken cancellationToken)
    {
        logger.LogDebug("Processing /ls");

        List<SavedMessage> foundMessages;

        // Если категория не дана - ищем во всех
        if (context.Argument is null)
            foundMessages = await db.SavedMessages.Where(msg => msg.ChatId == context.Message.Chat.Id).ToListAsync(cancellationToken: cancellationToken);
        // Если категория дана - ищем по ней (чтобы начиналось с категории)
        else
            foundMessages = await db.SavedMessages.Where(msg => msg.ChatId == context.Message.Chat.Id && EF.Functions.Like(msg.Name, $"{context.Argument}%")).ToListAsync(cancellationToken: cancellationToken);

        // Получаем все монтирования пользователя
        var mounts = await db.UserMounts.Where(mnt => mnt.UserId == context.Message.Chat.Id).ToListAsync();

        logger.LogDebug("/ls - found {count} mounts", mounts.Count);

        //Ищем сообщения в примонтированых чатах
        foreach (var mount in mounts)
        {
            if (context.Argument is null)
                foundMessages.AddRange(await db.SavedMessages.Where(msg => msg.ChatId == mount.ChatId).ToListAsync(cancellationToken: cancellationToken));
            else
                foundMessages.AddRange(await db.SavedMessages.Where(msg => msg.ChatId == mount.ChatId && EF.Functions.Like(msg.Name, $"{context.Argument}%")).ToListAsync(cancellationToken: cancellationToken));
        }

        logger.LogDebug("/ls - found {count} messages", foundMessages.Count);

        var msgBuilder = new StringBuilder();

        // Красиво записываем список сохраненных сообщений
        foreach (var msg in foundMessages)
        {
            msgBuilder.Append(" - ");
            msgBuilder.Append(mounts.Count(mnt => mnt.ChatId == msg.ChatId) == 0 ? msg.Name : $"{mounts.First(mnt => mnt.ChatId == msg.ChatId).Name}/{msg.Name}");
            msgBuilder.Append(" | [");
            msgBuilder.Append(msg.AddedByName);
            msgBuilder.Append("](tg://user?id=");
            msgBuilder.Append(msg.AddedById);
            msgBuilder.Append(") | ");
            msgBuilder.Append(msg.AddedAtUtc);
            msgBuilder.AppendLine();
        }

        logger.LogDebug("/ls - success");

        await botClient.SendTextMessageAsync(context.Message.Chat.Id, $"{textConsts.Value.LsSuccessMessage}\n{msgBuilder}", parseMode: ParseMode.Markdown, cancellationToken: cancellationToken);
    }
}