using FbkiBot.Attributes;
using FbkiBot.Configuration;
using FbkiBot.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FbkiBot.Commands;

[BotCommand("/start", "Выводит приветственное сообщение")]
public class StartCommand(ILogger<StartCommand> logger, IOptions<TextConstSettings> textConsts) : IChatCommand
{
    public bool CanExecute(CommandContext context) => context.Command?.Equals("/start", StringComparison.OrdinalIgnoreCase) ?? false;

    public async Task ExecuteAsync(ITelegramBotClient botClient, CommandContext context, CancellationToken cancellationToken)
    {
        logger.LogDebug("Sending welcome message");

        await botClient.SendTextMessageAsync(context.Message.Chat.Id, textConsts.Value.WelcomeMessage, cancellationToken: cancellationToken);
    }
}