using Telegram.Bot;
using Telegram.Bot.Types;

namespace FbkiBot.Commands;

/// <summary>
/// Команда, обрабатываемая чатботом
/// </summary>
public interface IChatCommand
{
    /// <summary>
    /// Стоит ли обрабатывать команду при данном сообщении
    /// </summary>
    /// <param name="message">Полученное сообщение</param>
    /// <returns>true если сообщение стоит обработать, иначе false</returns>
    bool CanExecute(Message message);

    /// <summary>
    /// Выполнить команду
    /// </summary>
    /// <param name="botClient">Клиент Telegram-бота</param>
    /// <param name="message">Полученное сообщение</param>
    /// <param name="cancellationToken">Токен для отмены действий</param>
    Task ExecuteAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken);

    /// <summary>
    /// Получить список аргументов после команды
    /// </summary>
    /// <param name="message">Сообщение, из которого нужно получить аргументы</param>
    /// <returns>Массив аргументов</returns>
    protected static string[] GetArgs(Message message) => message.Text!.Split(' ').Skip(1).ToArray();

    /// <summary>
    /// Получить аргументы после команды одной строкой
    /// </summary>
    /// <param name="message">Сообщение, из которого нужно получить аргументы</param>
    /// <returns>Все аргументы после команды одной строкой</returns>
    protected static string? GetArg(Message message)
    {
        var spaceIndex = message.Text!.IndexOf(' ');
        if (spaceIndex == -1) return null;

        return message.Text![(spaceIndex + 1)..];
    }
}