using FbkiBot.Models;
using Telegram.Bot;

namespace FbkiBot.Commands;

/// <summary>
///     Команда, обрабатываемая чатботом
/// </summary>
public interface IChatCommand
{
    /// <summary>
    ///     Стоит ли обрабатывать команду при данном сообщении
    /// </summary>
    /// <param name="context">Контекст команды</param>
    /// <returns>true если сообщение стоит обработать, иначе false</returns>
    bool CanExecute(CommandContext context);

    /// <summary>
    ///     Выполнить команду
    /// </summary>
    /// <param name="botClient">Клиент Telegram-бота</param>
    /// <param name="context">Контекст команды</param>
    /// <param name="cancellationToken">Токен для отмены действий</param>
    Task ExecuteAsync(ITelegramBotClient botClient, CommandContext context, CancellationToken cancellationToken);
}