using System.Diagnostics;
using FbkiBot.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FbkiBot.Middleware;

/// <summary>
///     ПО промежуточного слоя, замеряющее время обработки запросов
/// </summary>
public class RequestTimerMiddleware : IBotMiddleware
{
    /// <summary>
    ///     Замерить и залоггировать время обработки запроса
    /// </summary>
    public async Task InvokeAsync(UpdateContext context, BotMiddlewareDelegate next)
    {
        var logger = context.Services.GetRequiredService<ILogger<Program>>();

        var sw = new Stopwatch();
        sw.Start();

        await next(context);

        sw.Stop();
        logger.LogDebug("Update processed in {time}", sw.Elapsed);
    }
}