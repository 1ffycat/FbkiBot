using Telegram.Bot.Types;

namespace FbkiBot.Models;

/// <summary>
///     Контекст вызываемой команды
/// </summary>
/// <param name="message">Изначальное полученное сообщение</param>
/// <param name="command">Команда (первый токен текста)</param>
/// <param name="arguments">Аргументы (оставшиеся токены текста)</param>
public class CommandContext(Message message, string? command, string[] arguments)
{
    /// <summary>
    ///     Лениво-вычисляемая строка, содержащая все аргументы после команды виде одной строки
    /// </summary>
    private readonly Lazy<string?> _argument = new(arguments.Length > 0 ? string.Join(' ', arguments) : null);

    /// <summary>
    ///     Изначальное полученное сообщение
    /// </summary>
    public Message Message { get; set; } = message;

    /// <summary>
    ///     Является ли сообщение командой
    /// </summary>
    public bool IsCommand => Command is not null;

    /// <summary>
    ///     Текстовая команда (первый токен)
    ///     !! может, но не обязана начинаться с '/' !!
    /// </summary>
    public string? Command { get; set; } = command;

    /// <summary>
    ///     Аргументы команды (слова после самой команды)
    /// </summary>
    public string[] Arguments { get; set; } = arguments;

    /// <summary>
    ///     Все аргументы после команды в виде одной строки. Null если нет аргументов
    /// </summary>
    public string? Argument => _argument.Value;
}