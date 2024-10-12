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
        // Если сообщение пустое - у нас нет ни команды ни аргументов
        if (string.IsNullOrEmpty(message.Text)) return new(message, null, []);

        // Разбиваем сообщение на слова. Удаляем пустые токены, которые появляются в случае двух пробелов подряд
        var tokens = message.Text?.Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? [];
        // Получаем команду
        string? command = tokens.First();

        // Если команда найдена и в ней присутствует символ @ (команда по типу /help@fbkibot)
        if (command is not null && command.IndexOf('@') is int atId && atId != -1)
        {
            // Обрезаем, оставляя только часть до @
            command = atId == -1 ? command : command[..atId];
        }

        // Если команда не начинается со / (обычное сообщение) - не разделяем на команду и аргументы
        if (!command.StartsWith('/')) return new(message, null, tokens);
        // Если слеш-команда - разделяем на команду и аргументы
        else return new(message, command, tokens[1..]);
    }
}