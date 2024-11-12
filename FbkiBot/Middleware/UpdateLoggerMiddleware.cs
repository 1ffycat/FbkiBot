using FbkiBot.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FbkiBot.Middleware;

/// <summary>
/// ПО промежуточного слоя для логгирования полученных ботом событий
/// </summary>
public class UpdateLoggerMiddleware : IBotMiddleware
{
    /// <summary>
    /// Логгирует событие и продолжает выполнение пайплайна
    /// </summary>
    /// <param name="context">Контекст события</param>
    /// <param name="next">Следующий middleware</param>
    public async Task InvokeAsync(UpdateContext context, BotMiddlewareDelegate next)
    {
        var logger = context.Services.GetRequiredService<ILogger<UpdateLoggerMiddleware>>();

        logger.LogInformation("Received update: {@update}", context.Update);

        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while processing update {@update}", context.Update);
            throw;
        }
    }
}