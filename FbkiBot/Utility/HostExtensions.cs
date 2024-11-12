using FbkiBot.Middleware;
using FbkiBot.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FbkiBot.Utility;

public static class HostExtensions
{
    /// <summary>
    /// Использовать типизированный middleware
    /// </summary>
    /// <param name="host">Приложение</param>
    /// <typeparam name="T">Тип middleware</typeparam>
    /// <exception cref="NullReferenceException">Пайплайн middleware не был добавлен в DI перед добавлением ПО промежуточного слоя</exception>
    public static IHost Use<T>(this IHost host) where T : IBotMiddleware, new()
    {
        var pipeline = host.Services.GetService<BotMiddlewarePipeline>();

        if (pipeline is null) throw new NullReferenceException("Middleware pipeline is not found in the DI container.");

        pipeline.Register(new T());

        return host;
    }

    /// <summary>
    /// Использовать анонимный middleware
    /// </summary>
    /// <param name="host">Приложение</param>
    /// <param name="middleware">Обработчик middleware</param>
    /// <exception cref="NullReferenceException">Пайплайн middleware не был добавлен в DI перед добавлением ПО промежуточного слоя</exception>
    public static IHost Use(this IHost host, Func<UpdateContext, BotMiddlewareDelegate, Task> middleware)
    {
        var pipeline = host.Services.GetService<BotMiddlewarePipeline>();

        if (pipeline is null) throw new NullReferenceException("Middleware pipeline is not found in the DI container.");

        pipeline.Register(middleware);

        return host;
    }

    /// <summary>
    /// Использовать обработчик текстовых команд в приложении
    /// </summary>
    /// <param name="host">Приложение</param>
    public static IHost UseTextCommands(this IHost host) => host.Use<TextCommandExecuterMiddleware>();

    /// <summary>
    /// Использовать логгирование событий Telegram-бота в приложении
    /// </summary>
    /// <param name="host">Приложение</param>
    public static IHost UseUpdateLogger(this IHost host) => host.Use<UpdateLoggerMiddleware>();

    /// <summary>
    /// Использовать обработчик ошибок в приложении
    /// </summary>
    /// <param name="host">Приложение</param>
    /// <returns></returns>
    public static IHost UseErrorHandler(this IHost host) => host.Use<ErrorHandlerMiddleware>();

    /// <summary>
    /// Использовать таймер времени обработки обновления в приложении
    /// </summary>
    /// <param name="host">Приложение</param>
    public static IHost UseRequestTimer(this IHost host) => host.Use<RequestTimerMiddleware>();
}