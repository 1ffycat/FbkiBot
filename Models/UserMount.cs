using Telegram.Bot.Types;

namespace FbkiBot.Models;

/// <summary>
/// Сохраненное сообщение
/// </summary>
public class UserMount
{
    /// <summary>
    /// ID в БД
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// ID пользователя
    /// </summary>
    public long UserId { get; set; }


    /// <summary>
    /// ID чата
    /// </summary>
    public long ChatId { get; set; }

    /// <summary>
    /// Название mount
    /// </summary>
    public string Name { get; set; }


    /// <summary>
    /// Создать модель сохраненного сообщения
    /// </summary>
    /// <param name="name">Название mount</param>
    /// <param name="chatId">ID чата который примонтитровал пользователь</param>
    /// <param name="UserId">ID пользователя</param>
    public UserMount(string name, long chatId, long userId)
    {
        Name = name;     
        ChatId = chatId;
        UserId = userId;
    }

    public UserMount(string name, long chatId, User user) : this(name, chatId, user.Id) { }
}