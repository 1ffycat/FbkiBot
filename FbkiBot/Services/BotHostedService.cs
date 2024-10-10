using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FbkiBot.Services;

/// <summary>
/// Hosted-сервис, держащий сервис чатбота
/// </summary>
/// <param name="botService">Сервис бота</param>
/// <param name="logger">Логгер для внутреннего использования</param>
public class BotHostedService(IBotService botService, ILogger<BotHostedService> logger) : IHostedService
{
    /// <summary>
    /// Запустить сервис чатбота
    /// </summary>
    /// <param name="cancellationToken">Токен для отмены действий внутри сервиса чатбота</param>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Bot is starting...");
        await botService.StartAsync(cancellationToken);
    }

    /// <summary>
    /// Остановить сервис чатбота
    /// </summary>
    /// <param name="cancellationToken">Токен для отмены действий внутри сервиса чатбота</param>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Bot is stopping...");

        return Task.CompletedTask;
    }
}