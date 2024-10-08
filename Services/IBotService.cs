namespace FbkiBot.Services;

/// <summary>
/// Интерфейс сервиса бота, независим от площадки
/// </summary>
public interface IBotService
{
    /// <summary>
    /// Запустить бота
    /// </summary>
    /// <param name="cancellationToken">Токен для отмены действий в боте</param>
    /// <returns></returns>
    Task StartAsync(CancellationToken cancellationToken);
}