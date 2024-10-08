using FbkiBot.Attributes;
using FbkiBot.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FbkiBot.Commands;

[BotCommand("/start", "Выводит приветственное сообщение")]
public class StartCommand(ILogger<StartCommand> logger, IOptions<TextConstSettings> textConsts) : IChatCommand
{
    public bool CanExecute(Message message) => string.Equals(message.Text, "/start", StringComparison.OrdinalIgnoreCase);

    public async Task ExecuteAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        logger.LogDebug("Sending welcome message");
        await botClient.SendTextMessageAsync(message.Chat.Id, textConsts.Value.WelcomeMessage, cancellationToken: cancellationToken);
    }
}