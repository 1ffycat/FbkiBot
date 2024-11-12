using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace FbkiBot.Utility;

public static class ConfigurationManagerExtensions
{
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