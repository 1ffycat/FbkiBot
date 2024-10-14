using FbkiBot.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
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

    /// <summary>
    /// Добавить JSON файл в конфигурацию с наименьшим приоритетом (любой другой конфиг, добавленный ранее, будет иметь приоритет над ним)
    /// </summary>
    /// <param name="configuration">Менеджер конфигурации</param>
    /// <param name="path">Путь до файла</param>
    /// <param name="optional">Опционален (необязателен) ли файл?</param>
    /// <param name="reloadOnChange">Автоматически обновлять конфигурацию при изменении файла?</param>
    /// <remarks>Метод устанавливает файлу минимальный приоритет из имеющихся. Если вызвать этот метод на два разных файла, высший приоритет будет иметь тот, который был добавлен ранее</remarks>
    /// <returns></returns>
    public static IConfigurationManager AddDefaultsJsonFile(this IConfigurationManager configuration, string path, bool optional = false, bool reloadOnChange = false)
    {
        configuration.Sources.Insert(0, new JsonConfigurationSource() { Path = path, Optional = optional, ReloadOnChange = reloadOnChange });

        return configuration;
    }
}