using FbkiBot.Attributes;
using FbkiBot.Models;
using FbkiBot.Resources;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace FbkiBot.Commands;

[BotCommand("/start", "Выводит приветственное сообщение")]
public class StartCommand(ILogger<StartCommand> logger) : IChatCommand
{
    public bool CanExecute(CommandContext context)
    {
        return context.Command?.Equals("/start", StringComparison.OrdinalIgnoreCase) ?? false;
    }

    public async Task ExecuteAsync(ITelegramBotClient botClient, CommandContext context,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Sending welcome message");

        await botClient.SendTextMessageAsync(context.Message.Chat.Id, CommandStrings.Start_Welcome,
            cancellationToken: cancellationToken);
    }
}