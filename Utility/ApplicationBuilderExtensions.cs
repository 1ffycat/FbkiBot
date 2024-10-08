using FbkiBot.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FbkiBot.Utility;

/// <summary>
/// Расширения для билдера приложения
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Зарегистрировать команду в чатботе
    /// </summary>
    /// <typeparam name="T">Тип команды</typeparam>
    /// <param name="app">Билдер приложения, в которое нужно добавить команду</param>
    /// <returns>Билдер приложения с добавленной командой</returns>
    public static IHostApplicationBuilder AddCommand<T>(this IHostApplicationBuilder app) where T : class, IChatCommand
    {
        app.Services.AddTransient<IChatCommand, T>();

        return app;
    }

    /// <summary>
    /// Добавить настройки для парсинга
    /// </summary>
    /// <typeparam name="T">Тип модели настроек</typeparam>
    /// <param name="app">Билдер приложения, в которое нужно добавить настройки</param>
    /// <param name="name">Название секции настроек</param>
    /// <returns>Билдер приложения с добавленными настройками</returns>
    public static IHostApplicationBuilder AddSettings<T>(this IHostApplicationBuilder app, string name) where T : class
    {
        app.Services.Configure<T>(app.Configuration.GetSection(name));

        return app;
    }
}