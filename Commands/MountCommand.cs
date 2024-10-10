using FbkiBot.Attributes;
using FbkiBot.Configuration;
using FbkiBot.Data;
using FbkiBot.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using FbkiBot.Commands;

[BotCommand("/mount", "Связывает сохранения из чата с личной перепиской с ботом", "/mount <название>")]
public class MountCommand(BotDbContext db, ILogger<MountCommand> logger, IOptions<TextConstSettings> textConsts) : IChatCommand
{
    public bool CanExecute(CommandContext context) => context.Command?.Equals("/mount", StringComparison.OrdinalIgnoreCase) ?? false;

    public async Task ExecuteAsync(ITelegramBotClient botClient, CommandContext context, CancellationToken cancellationToken)
    {
        logger.LogDebug("Processing mount command");

        if (context.Message.Chat.Id == context.Message.From!.Id)
        {
            await botClient.SendTextMessageAsync(context.Message.Chat.Id, "Команда вызвана из личного чата пользователя", cancellationToken: cancellationToken);
            return;
        }

        // Если не задано название
        if (context.Argument is null)
        {
            logger.LogDebug("/mount denied - no context.Argument");
            await botClient.SendTextMessageAsync(context.Message.Chat.Id, "mount не дано название", cancellationToken: cancellationToken);
            return;
        }

        // Если mount с таким названием уже существует
        if (await db.FindUserMountAsync(context.Argument, context.Message.From.Id, cancellationToken) is UserMount existingMount)
        {
            await botClient.SendTextMessageAsync(context.Message.Chat.Id, "mount с таким названием уже существует", cancellationToken: cancellationToken);
            return;
        }

        // Если mount для этого чата уже существует
        if (db.UserMounts.Count(mnt => mnt.UserId == context.Message.From.Id || mnt.ChatId == context.Message.Chat.Id)!=0){
            await botClient.SendTextMessageAsync(context.Message.Chat.Id, "mount для этого чата уже существует", cancellationToken: cancellationToken);
            return;
        }

        var userMount = new UserMount(context.Argument, context.Message.Chat.Id, context.Message.From);

        logger.LogDebug("Saving message to db...");

        await db.UserMounts.AddAsync(userMount, cancellationToken: cancellationToken);
        await db.SaveChangesAsync(cancellationToken: cancellationToken);

        logger.LogDebug("/mount - success");
        await botClient.SendTextMessageAsync(context.Message.Chat.Id, "mount Успешно добавлен", cancellationToken: cancellationToken);
    }
}