using Telegram.Bot;
using Telegram.Bot.Types;

namespace FbkiBot.Models;

/// <summary>
///     Контекст события Telegram бота
/// </summary>
/// <param name="update">Само событие</param>
/// <param name="client">Клиент Telegram-бота</param>
/// <param name="services">Сервисы приложения</param>
/// <param name="ct">Токен для отмены выполнения</param>
public class UpdateContext(Update update, ITelegramBotClient client, IServiceProvider services, CancellationToken ct)
{
    /// <summary>
    ///     Событие Telegram-бота
    /// </summary>
    public Update Update { get; set; } = update;

    /// <summary>
    ///     Клиент Telegram-бота, из которого получено событие
    /// </summary>
    public ITelegramBotClient Client { get; set; } = client;

    /// <summary>
    ///     Провайдер сервисов приложения
    /// </summary>
    public IServiceProvider Services { get; set; } = services;

    /// <summary>
    ///     Токен для отмены выполнения
    /// </summary>
    public CancellationToken CancellationToken { get; set; } = ct;
}