using FbkiBot.Attributes;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FbkiBot.Commands;

[BotCommand("/start", "Выводит приветственное сообщение")]
public class StartCommand(ILogger<StartCommand> logger) : IChatCommand
{
    public bool CanExecute(Message message) => string.Equals(message.Text, "/start", StringComparison.OrdinalIgnoreCase);

    public async Task ExecuteAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        logger.LogDebug("Received /start command. Sending welcome message");
        await botClient.SendTextMessageAsync(message.Chat.Id, "Bot is running", cancellationToken: cancellationToken);
    }
}