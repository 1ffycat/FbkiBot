using FbkiBot.Models;

namespace FbkiBot.Middleware;

/// <summary>
///     ПО промежуточного слоя для обработки событий бота
/// </summary>
public interface IBotMiddleware
{
    /// <summary>
    ///     Выполнить обработку и, опционально, продолжить выполнение пайплайна
    /// </summary>
    /// <param name="context">Контекст события</param>
    /// <param name="next">Следующий middleware</param>
    Task InvokeAsync(UpdateContext context, BotMiddlewareDelegate next);
}

/// <summary>
///     Делегат вызова middleware
/// </summary>
public delegate Task BotMiddlewareDelegate(UpdateContext context);