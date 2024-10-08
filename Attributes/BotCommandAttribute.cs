namespace FbkiBot.Attributes;

/// <summary>
/// Атрибут, описывающий команду чатбота
/// </summary>
/// <param name="name">Название команды. Например, /start</param>
/// <param name="description">Описание того, что делает команда</param>
/// <param name="usage">Пример использования команды. Опционально</param>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class BotCommandAttribute(string name, string description, string? usage = null) : Attribute
{
    /// <summary>
    /// Название команды
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// Описание команды
    /// </summary>
    public string Description { get; } = description;

    /// <summary>
    /// Пример использования команды
    /// </summary>
    public string? Usage { get; } = usage;
}

// TODO: мб вынести описание и использование команд в конфигурацию, чтобы можно было менять без пересборки