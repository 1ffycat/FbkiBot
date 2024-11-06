using System.ComponentModel.DataAnnotations;
using Telegram.Bot.Types;

namespace FbkiBot.Models;

/// <summary>
/// Сохраненное сообщение
/// </summary>
public class SavedMessage
{
    /// <summary>
    /// ID в БД
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Дата добавления
    /// </summary>
    public DateTime AddedAtUtc { get; set; }

    /// <summary>
    /// ID автора
    /// </summary>
    public long AddedById { get; set; }

    /// <summary>
    /// Username автора
    /// </summary>
    [MaxLength(32)]
    public string? AddedByUsername { get; set; }

    /// <summary>
    /// Имя автора
    /// </summary>
    [MaxLength(512)]
    public string AddedByName { get; set; }

    /// <summary>
    /// ID чата, в котором сохранено сообщение
    /// </summary>
    public long ChatId { get; set; }

    /// <summary>
    /// Название сохраненного сообщения
    /// </summary>
    [MaxLength(128)]
    public string Name { get; set; }

    /// <summary>
    /// ID сообщения, которое было сохранено
    /// </summary>
    public int MessageId { get; set; }

    /// <summary>
    /// Создать модель сохраненного сообщения
    /// </summary>
    /// <param name="name">Название</param>
    /// <param name="messageId">ID сохраняемого сообщения</param>
    /// <param name="chatId">ID чата, в котором сохраняем</param>
    /// <param name="addedById">ID пользователя который сохраняет сообщение</param>
    /// <param name="addedByUsername">Username пользователя который сохраняет сообщение</param>
    /// <param name="addedByName">Имя пользователя который сохраняет сообщение</param>
    public SavedMessage(string name, int messageId, long chatId, long addedById, string? addedByUsername, string addedByName)
    {
        Name = name;
        MessageId = messageId;
        ChatId = chatId;
        AddedById = addedById;
        AddedByUsername = addedByUsername;
        AddedByName = addedByName;

        // Ставим дату сохранения на текущую по UTC
        AddedAtUtc = DateTime.UtcNow;
    }

    public SavedMessage(string name, int messageId, long chatId, User author) : this(name, messageId, chatId, author.Id, author.Username, $"{author.FirstName} {author.LastName}") { }
}