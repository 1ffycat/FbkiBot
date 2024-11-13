using FbkiBot.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FbkiBot.Utility;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Зарегистрировать команду в чатботе
    /// </summary>
    /// <typeparam name="T">Тип команды</typeparam>
    /// <param name="services">Коллекция сервисов приложения</param>
    /// <returns>Коллекция сервисов приложения приложения с добавленной командой</returns>
    public static IServiceCollection AddCommand<T>(this IServiceCollection services) where T : class, IChatCommand
    {
        services.AddTransient<IChatCommand, T>();

        return services;
    }

    /// <summary>
    ///     Добавить настройки для парсинга
    /// </summary>
    /// <typeparam name="T">Тип модели настроек</typeparam>
    /// <param name="services">Сервисы приложения</param>
    /// <param name="section">Секция настроек, к которой нужно привязать класс</param>
    /// <returns>Сервисы приложения с добавленными настройками</returns>
    public static IServiceCollection AddSettings<T>(this IServiceCollection services, IConfigurationSection section)
        where T : class
    {
        services.Configure<T>(section);

        return services;
    }
}