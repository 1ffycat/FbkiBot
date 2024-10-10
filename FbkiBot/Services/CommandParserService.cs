using FbkiBot.Models;
using Telegram.Bot.Types;

namespace FbkiBot.Services;

/// <summary>
/// Занимается парсингом сообщения на команду и аргументы
/// </summary>
public class CommandParserService
{
    /// <summary>
    /// Парсит сообщение на команду и токены
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public CommandContext BuildContext(Message message)
    {
        // Разбиваем сообщение на слова
        var tokens = message.Text?.Split(' ') ?? [];
        // Получаем команду
        string? command = tokens.First();
        // Если команда не начинается со / (обычное сообщение) - не разделяем на команду и аргументы
        if (!command.StartsWith('/')) return new(message, null, tokens);
        // Если слеш-команда - разделяем на команду и аргументы
        else return new(message, command, tokens[1..]);
    }
}