using FbkiBot.Attributes;
using FbkiBot.Configuration;
using FbkiBot.Models;
using FbkiBot.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FbkiBot.Commands;

[BotCommand("/start", "Выводит приветственное сообщение")]
public class StartCommand(BotDbContext db, ILogger<StartCommand> logger, IOptions<TextConstSettings> textConsts) : IChatCommand
{
    public bool CanExecute(CommandContext context) => context.Command?.Equals("/start", StringComparison.OrdinalIgnoreCase) ?? false;

    public async Task ExecuteAsync(ITelegramBotClient botClient, CommandContext context, CancellationToken cancellationToken)
    {
        logger.LogDebug("Sending welcome message");

        // Если монтирования нет
        if (await db.FindUserMountAsync("", context.Message.From!.Id, cancellationToken) is not UserMount existingMount)
        {
            var userMount = new UserMount("", context.Message.Chat.Id, context.Message.From!);

            logger.LogDebug("Saving UserMount to db...");

            await db.UserMounts.AddAsync(userMount, cancellationToken: cancellationToken);
            await db.SaveChangesAsync(cancellationToken: cancellationToken);
        }
        await botClient.SendTextMessageAsync(context.Message.Chat.Id, textConsts.Value.WelcomeMessage, cancellationToken: cancellationToken);
    }
}