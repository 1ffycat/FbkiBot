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

        // Выбираем из таблицы сохраненных сообщений те у который Id чата равен чату откуда пришло сообщение
        // И создаем класс найденный сообщений в который входит класс сохраненных сообщений и имя монтирования равно null
        if (context.Argument is null)           
            foundMessages = await db.SavedMessages.Where(msg => msg.ChatId == context.Message.Chat.Id)
            .Select(msg => new FoundMessage(msg, null))
            .ToListAsync(cancellationToken: cancellationToken);
        // Тоже самое что и выше но фильтруем еще по категории
        else
            foundMessages = await db.SavedMessages.Where(msg => msg.ChatId == context.Message.Chat.Id && EF.Functions.Like(msg.Name, $"{context.Argument}%"))
            .Select(msg => new FoundMessage(msg, null))
            .ToListAsync(cancellationToken: cancellationToken);

        if (context.Message.Chat.Type == ChatType.Private)
        {
            // Делаем inner join таблицы сохраненный сообщений и таблицы монтирований определенного пользователя, по Id чата
            // И создаем класс найденный сообщений в который входит класс сохраненных сообщений и имя монтирования по которому найдено сообщение
            var userSavedMessages = await db.SavedMessages.Join(
                db.UserMounts.Where(mnt => mnt.UserId == context.Message.From!.Id),
                msg => msg.ChatId,
                mnt => mnt.ChatId,
                (msg, mnt) => new FoundMessage(msg, mnt.Name))
                .ToListAsync();

            // Если категория не дана добавляем все найденные сообщения по монтированиям
            if (context.Argument is null)
                foundMessages.AddRange(userSavedMessages);
            // Если категория дана филтруем найденные сообщения по монтированиям по категории
            else
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