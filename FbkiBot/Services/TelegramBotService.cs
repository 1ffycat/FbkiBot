using System.Reflection;
using FbkiBot.Attributes;
using FbkiBot.Commands;
using FbkiBot.Configuration;
using FbkiBot.Middleware;
using FbkiBot.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace FbkiBot.Services;

/// <summary>
///     Сервис Telegram-бота
/// </summary>
/// <param name="tgSettings">Специфичные для Telegram настройки (в т.ч. BotToken)</param>
/// <param name="serviceProvider">DI-провайдер сервисов</param>
/// <param name="logger">Логгер для внутреннего использования</param>
/// <param name="pipeline">Pipeline для ПО промежуточного слоя</param>
public class TelegramBotService(
    IOptions<TelegramSettings> tgSettings,
    IServiceProvider serviceProvider,
    ILogger<TelegramBotService> logger,
    BotMiddlewarePipeline pipeline) : IHostedService
{
    private readonly TelegramBotClient _botClient = new(tgSettings.Value.BotToken);

    private BotMiddlewareDelegate? _pipeline;

    /// <summary>
    ///     Запустить обработку событий Telegram-бота
    /// </summary>
    /// <param name="cancellationToken">Токен для остановки действий</param>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _pipeline = pipeline.Build();

        // Объявляем Телеграму о всех доступных командах
        await SetBotCommandsAsync(cancellationToken);

        var receiverOptions = new ReceiverOptions();

        _botClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cancellationToken
        );
        logger.LogInformation("Bot started receiving");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("The bot is shutting down");
        return Task.CompletedTask;

        // Telegram в режиме Polling не предполагает никакой финализации
    }

    /// <summary>
    ///     Добавить все команды с атрибутом BotCommand в список команд бота в Telegram.
    /// </summary>
    /// <param name="cancellationToken">Токен для отмены</param>
    /// <returns></returns>
    private async Task SetBotCommandsAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug("Evaluating all available commands and their descriptions");

        await using var scope = serviceProvider.CreateAsyncScope();

        var commands = scope.ServiceProvider.GetServices<IChatCommand>();

        var botCommands = commands.Select(c => c.GetType().GetCustomAttribute<BotCommandAttribute>())
            .Where(c => c is not null)
            .Select(c => new BotCommand
            {
                Command = c!.Name,
                Description = $"{c.Description} {(c.Usage is null ? "" : $"({c.Usage})")}"
            }).ToArray();

        logger.LogDebug("Found {commandCount} commands", botCommands.Length);

        await _botClient.SetMyCommands(botCommands, cancellationToken: cancellationToken);

        logger.LogInformation("Advertised {commandCount} available commands to Telegram", botCommands.Length);
    }

    /// <summary>
    ///     Обработать полученное ботом событие
    /// </summary>
    /// <param name="botClient">Клиент Telegram-бота</param>
    /// <param name="update">Полученное событие</param>
    /// <param name="cancellationToken">Токен для отмены действий</param>
    /// <exception cref="ArgumentNullException">Pipeline для ПО промежуточного слоя не был построен перед запуском бота</exception>
    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        if (_pipeline is null) throw new NullReferenceException("Pipeline was not built.");

        // Посылаем контекст события в первый middleware
        await _pipeline(new UpdateContext(update, botClient, serviceProvider, cancellationToken));

        // logger.LogDebug("Received message from {id}. Content: {text}", message.Chat.Id, message.Text);
        //
        // // Парсим команду и аргументы из сообщения
        // var context = cmdParser.BuildContext(message);
        //
        // using var scope = serviceProvider.CreateAsyncScope();  // FIXME: Где-то здесь создается новое подключение к БД при каждом сообщении - даже если не команда
        //
        // var commands = scope.ServiceProvider.GetServices<IChatCommand>();
        //
        // foreach (var command in commands)
        // {
        //     if (command.CanExecute(context))
        //     {
        //         await command.ExecuteAsync(botClient, context, cancellationToken);
        //         break;
        //     }
        // }
    }

    /// <summary>
    ///     Обрабатывает произошедшую в боте ошибку. Логгирует событие и продолжает выполнение.
    /// </summary>
    /// <param name="botClient">Клиент Telegram-бота</param>
    /// <param name="exception">Данные о произошедшей ошибке</param>
    /// <param name="cancellationToken">Токен для отмены действий</param>
    private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException =>
                $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        logger.LogError("Telegram bot error: {errorMessage}", errorMessage);
        return Task.CompletedTask;
    }
}