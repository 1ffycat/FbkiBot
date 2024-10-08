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
}