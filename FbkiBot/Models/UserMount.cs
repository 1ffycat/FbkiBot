using System.ComponentModel.DataAnnotations;
using Telegram.Bot.Types;

namespace FbkiBot.Models;

/// <summary>
///     Монтирование групповых чатов в личную переписку с ботом
/// </summary>
public class UserMount
{
    /// <summary>
    ///     Создать модель сохраненного сообщения
    /// </summary>
    /// <param name="name">Название монтирования</param>
    /// <param name="chatId">ID чата который примонтитровал пользователь</param>
    /// <param name="userId">ID пользователя</param>
    public UserMount(string name, long chatId, long userId)
    {
        Name = name;
        ChatId = chatId;
        UserId = userId;
    }

    public UserMount(string name, long chatId, User user) : this(name, chatId, user.Id)
    {
    }

    /// <summary>
    ///     ID в БД
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     ID пользователя который монтирует чат
    /// </summary>
    public long UserId { get; set; }


    /// <summary>
    ///     ID чата который монтируется
    /// </summary>
    public long ChatId { get; set; }

    /// <summary>
    ///     Название монтирования
    /// </summary>
    [MaxLength(64)]
    public string Name { get; set; }
}