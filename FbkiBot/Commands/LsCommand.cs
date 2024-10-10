using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using FbkiBot.Attributes;
using FbkiBot.Configuration;
using FbkiBot.Data;
using FbkiBot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
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

        List<FoundMessages> foundMessages;

        // Если категория не дана - ищем во всех
        if (context.Argument is null)
            foundMessages = await db.SavedMessages.Where(msg => msg.ChatId == context.Message.Chat.Id)
            .Select(msg => new FoundMessages(msg.Name, msg.MessageId, msg.ChatId, msg.AddedById, msg.AddedByUsername, msg.AddedByName, msg.AddedAtUtc, null))
            .ToListAsync(cancellationToken: cancellationToken);
        // Если категория дана - ищем по ней (чтобы начиналось с категории)
        else
            foundMessages = await db.SavedMessages.Where(msg => msg.ChatId == context.Message.Chat.Id && EF.Functions.Like(msg.Name, $"{context.Argument}%"))
            .Select(msg => new FoundMessages(msg.Name, msg.MessageId, msg.ChatId, msg.AddedById, msg.AddedByUsername, msg.AddedByName, msg.AddedAtUtc, null))
            .ToListAsync(cancellationToken: cancellationToken);

        // Получаем все монтирования пользователя
        var mounts = await db.UserMounts.Where(mnt => mnt.UserId == context.Message.Chat.Id).ToListAsync();

        logger.LogDebug("/ls - found {count} mounts", mounts.Count);
        var userSavedMessages = await db.SavedMessages.Join(
            db.UserMounts.Where(mnt => mnt.UserId == context.Message.Chat.Id),
            msg => msg.ChatId,
            mnt => mnt.ChatId,
            (msg, mnt) => new FoundMessages(msg.Name, msg.MessageId, msg.ChatId, msg.AddedById, msg.AddedByUsername, msg.AddedByName, msg.AddedAtUtc, mnt.Name))
            .ToListAsync();

        //Ищем сообщения в примонтированых чатах
        if (context.Argument is null)
            foundMessages.AddRange(userSavedMessages);
        else
            foundMessages.AddRange(userSavedMessages.Where(msg => Regex.IsMatch(msg.MessageName, $"^{context.Argument}.*")));

        logger.LogDebug("/ls - found {count} messages", foundMessages.Count);

        var msgBuilder = new StringBuilder();

        // Красиво записываем список сохраненных сообщений
        foreach (var msg in foundMessages)
        {
            msgBuilder.Append(" - ");
            msgBuilder.Append(msg.MountName is null ? msg.MessageName : $"{msg.MountName}/{msg.MessageName}");
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