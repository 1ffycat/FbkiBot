using FbkiBot.Attributes;
using FbkiBot.Commands;
using FbkiBot.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Reflection;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace FbkiBot.Services;

/// <summary>
/// Сервис Telegram-бота
/// </summary>
/// <param name="tgSettings">Специфичные для Telegram настройки (в т.ч. BotToken)</param>
/// <param name="commands">Список всех включенных команд</param>
/// <param name="logger">Логгер для внутреннего использования</param>
public class TelegramBotService(IOptions<TelegramSettings> tgSettings, IServiceProvider serviceProvider, ILogger<TelegramBotService> logger, CommandParserService cmdParser) : IBotService
{
    private readonly TelegramBotClient _botClient = new(tgSettings.Value.BotToken);

    /// <summary>
    /// Запустить обработку событий Telegram-бота
    /// </summary>
    /// <param name="cancellationToken">Токен для остановки действий</param>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // Объявляем Телеграму о всех доступных командах
        await SetBotCommandsAsync(cancellationToken);

        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = { }
        };

        _botClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cancellationToken
        );
        logger.LogInformation("Bot started receiving");
    }

    /// <summary>
    /// Добавить все команды с атрибутом BotCommand в список команд бота в Telegram.
    /// </summary>
    /// <param name="cancellationToken">Токен для отмены</param>
    /// <returns></returns>
    private async Task SetBotCommandsAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug("Evaluating all available commands and their descriptions");

        using var scope = serviceProvider.CreateAsyncScope();

        var commands = scope.ServiceProvider.GetServices<IChatCommand>();

        var botCommands = commands.Select(c => c.GetType().GetCustomAttribute<BotCommandAttribute>())
            .Where(c => c is not null)
            .Select(c => new BotCommand()
            {
                Command = c!.Name,
                Description = $"{c.Description} {(c.Usage is null ? "" : $"({c.Usage})")}"
            }).ToArray();

        logger.LogDebug("Found {commandCount} commands", botCommands.Length);

        await _botClient.SetMyCommandsAsync(botCommands, cancellationToken: cancellationToken);

        logger.LogInformation("Advertised {commandCount} available commands to Telegram", botCommands.Length);
    }

    /// <summary>
    /// Обработать полученное ботом событие
    /// </summary>
    /// <param name="botClient">Клиент Telegram-бота</param>
    /// <param name="update">Полученное событие</param>
    /// <param name="cancellationToken">Токен для отмены действий</param>
    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not Message message || string.IsNullOrEmpty(update.Message.Text)) return;
        logger.LogDebug("Received message from {id}. Content: {text}", message.Chat.Id, message.Text);

        // Парсим команду и аргументы из сообщения
        var context = cmdParser.BuildContext(message);

        using var scope = serviceProvider.CreateAsyncScope();

        var commands = scope.ServiceProvider.GetServices<IChatCommand>();

        foreach (var command in commands)
        {
            if (command.CanExecute(context))
            {
                await command.ExecuteAsync(botClient, context, cancellationToken);
                break;
            }
        }
    }

    /// <summary>
    /// Обрабатывает произошедшую в боте ошибку. Логгирует событие и продолжает выполнение.
    /// </summary>
    /// <param name="botClient">Клиент Telegram-бота</param>
    /// <param name="exception">Данные о произошедшей ошибке</param>
    /// <param name="cancellationToken">Токен для отмены действий</param>
    private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        logger.LogError("Telegram bot error: {errorMessage}", errorMessage);
        return Task.CompletedTask;
    }
}