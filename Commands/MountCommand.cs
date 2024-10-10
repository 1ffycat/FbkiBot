using FbkiBot.Attributes;
using FbkiBot.Configuration;
using FbkiBot.Data;
using FbkiBot.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FbkiBot.Commands;

[BotCommand("/mount", "Связывает сохранения из чата с личной перепиской с ботом", "/mount <название>")]
public class MountCommand(BotDbContext db, ILogger<SaveCommand> logger, IOptions<TextConstSettings> textConsts) : IChatCommand
{
    public bool CanExecute(Message message) => message.Text!.StartsWith("/mount", StringComparison.OrdinalIgnoreCase);

    public async Task ExecuteAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        logger.LogDebug("Processing mount command");
        var name = IChatCommand.GetArg(message);  // Получаем текст после команды

        if (message.Chat.Id == message.From!.Id)
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, "Команда вызвана из личного чата пользователя", cancellationToken: cancellationToken);
            return;
        }

        // Если не задано название
        if (name is null)
        {
            logger.LogDebug("/mount denied - no name");
            await botClient.SendTextMessageAsync(message.Chat.Id, "mount не дано название", cancellationToken: cancellationToken);
            return;
        }

        // Если mount с таким названием уже существует
        if (await db.FindUserMountAsync(name, message.From.Id, cancellationToken) is UserMount existingMount)
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, "mount с таким названием уже существует", cancellationToken: cancellationToken);
            return;
        }

        // Если mount для этого чата уже существует
        if (db.UserMounts.Any(mnt => mnt.UserId == message.From.Id || mnt.ChatId == message.Chat.Id)){
            await botClient.SendTextMessageAsync(message.Chat.Id, "mount для этого чата уже существует", cancellationToken: cancellationToken);
            return;
        }

        var userMount = new UserMount(name, message.Chat.Id, message.From);

        logger.LogDebug("Saving message to db...");

        await db.UserMounts.AddAsync(userMount, cancellationToken: cancellationToken);
        await db.SaveChangesAsync(cancellationToken: cancellationToken);

        logger.LogDebug("/mount - success");
        await botClient.SendTextMessageAsync(message.Chat.Id, "mount Успешно добавлен", cancellationToken: cancellationToken);
    }
}