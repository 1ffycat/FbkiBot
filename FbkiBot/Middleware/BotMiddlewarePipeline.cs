using FbkiBot.Models;
using Microsoft.Extensions.Logging;

namespace FbkiBot.Middleware;

/// <summary>
/// Pipeline ПО промежуточного слоя
/// </summary>
/// <param name="logger">Логгер для внутреннего использования</param>
public class BotMiddlewarePipeline(ILogger<BotMiddlewarePipeline> logger)
{
    private readonly List<Func<BotMiddlewareDelegate, BotMiddlewareDelegate>> _components = [];

    /// <summary>
    /// Добавить анонимный middleware
    /// </summary>
    /// <param name="middleware">Обрабочтик middleware</param>
    public void Register(Func<UpdateContext, BotMiddlewareDelegate, Task> middleware)
    {
        _components.Add(next =>
        {
            return context => middleware(context, next);
        });
    }

    /// <summary>
    /// Добавить типизированный middleware
    /// </summary>
    /// <param name="middleware">Экземляр ПО промежуточного слоя</param>
    public void Register(IBotMiddleware middleware)
    {
        _components.Add(next =>
        {
            return context => middleware.InvokeAsync(context, next);
        });
    }

    /// <summary>
    /// Построить пайплайн
    /// </summary>
    /// <returns>Первый middleware в пайплайне</returns>
    public BotMiddlewareDelegate Build()
    {
        logger.LogDebug("Building pipeline with {middlewareCount} components: {@components}", _components.Count, _components);

        BotMiddlewareDelegate pipeline = _ => Task.CompletedTask;

        for (int i = _components.Count - 1; i >= 0; i--)
        {
            pipeline = _components[i](pipeline);
        }

        logger.LogDebug("Pipeline built with {middlewareCount} components. Starting with {@firstMiddleware}", _components.Count, pipeline);

        return pipeline;
    }
}