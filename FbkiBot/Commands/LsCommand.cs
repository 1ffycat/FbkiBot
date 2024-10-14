using FbkiBot.Attributes;
using FbkiBot.Configuration;
using FbkiBot.Data;
using FbkiBot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
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

        List<FoundMessage> foundMessages;

        // Если не передано категории
        if (context.Argument is null)
            // Находим монтирования для текущего чата
            foundMessages = await db.SavedMessages.Where(msg => msg.ChatId == context.Message.Chat.Id)  // ID соответствует текущему чату
            .Select(msg => new FoundMessage(msg, null))  // название монтирования оставляем пустым
            .ToListAsync(cancellationToken: cancellationToken);
        // Если категория передана - фильтруем по ней
        else
            // Сообщения с таким же ID чата, начинающиеся с переданной категории
            foundMessages = await db.SavedMessages.Where(msg => msg.ChatId == context.Message.Chat.Id && EF.Functions.Like(msg.Name, $"{context.Argument}%"))
            .Select(msg => new FoundMessage(msg, null))
            .ToListAsync(cancellationToken: cancellationToken);

        // Если сообщение в ЛС
        if (context.Message.Chat.Type == ChatType.Private)
        {
            // Добавляем в список найденных сообщений сохраненки из примонтированных чатов
            var userSavedMessages = await db.SavedMessages.Join(
                db.UserMounts.Where(mnt => mnt.UserId == context.Message.From!.Id),  // выбираем монтирования текущего пользователя
                msg => msg.ChatId,
                mnt => mnt.ChatId,
                (msg, mnt) => new FoundMessage(msg, mnt.Name))
                .ToListAsync(cancellationToken);

            // Если категория не дана - добавляем все сообщения из примонтированных чатов
            if (context.Argument is null)
                foundMessages.AddRange(userSavedMessages);
            // Если категория дана - фильтруем сообщения из примонтированных чатов по ней
            else
                // Добавляем только те сообщения, которые начинаются с данной категории
                foundMessages.AddRange(userSavedMessages.Where(msg => msg.Message.Name.StartsWith($"{context.Argument}", StringComparison.OrdinalIgnoreCase)).ToList());
        }

        logger.LogDebug("/ls - found {count} messages", foundMessages.Count);

        var msgBuilder = new StringBuilder();

        // Красиво записываем список сохраненных сообщений
        foreach (var msg in foundMessages)
        {
            msgBuilder.Append(" - ");
            msgBuilder.Append(msg.MountName is null ? msg.Message.Name : $"{msg.MountName}/{msg.Message.Name}");
            msgBuilder.Append(" | [");
            msgBuilder.Append(msg.Message.AddedByName);
            msgBuilder.Append("](tg://user?id=");
            msgBuilder.Append(msg.Message.AddedById);
            msgBuilder.Append(") | ");
            msgBuilder.Append(msg.Message.AddedAtUtc);
            msgBuilder.AppendLine();
        }

        logger.LogDebug("/ls - success");

        await botClient.SendTextMessageAsync(context.Message.Chat.Id, $"{textConsts.Value.LsSuccessMessage}\n{msgBuilder}", parseMode: ParseMode.Markdown, cancellationToken: cancellationToken);
    }
}