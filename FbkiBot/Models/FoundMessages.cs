using Telegram.Bot.Types;

namespace FbkiBot.Models;

/// <summary>
/// Сохраненное сообщение
/// </summary>
public class FoundMessages
{
    /// <summary>
    /// ID в БД
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Дата добавления
    /// </summary>
    public DateTime AddedAtUtc { get; set; }

    /// <summary>
    /// ID атвора
    /// </summary>
    public long AddedById { get; set; }

    /// <summary>
    /// Username автора
    /// </summary>
    public string? AddedByUsername { get; set; }

    /// <summary>
    /// Имя автора
    /// </summary>
    public string AddedByName { get; set; }

    /// <summary>
    /// ID чата, в котором сохранено сообщение
    /// </summary>
    public long ChatId { get; set; }

    /// <summary>
    /// Название сохраненного сообщения
    /// </summary>
    public string MessageName { get; set; }

    /// <summary>
    /// Название монтирования
    /// </summary>
    public string? MountName { get; set; }

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
    public FoundMessages(string messageName, int messageId, long chatId, long addedById, string? addedByUsername, string addedByName, DateTime addedAtUtc, string? mountName)
    {
        MountName = mountName;
        MessageName = messageName;
        MessageId = messageId;
        ChatId = chatId;
        AddedById = addedById;
        AddedByUsername = addedByUsername;
        AddedByName = addedByName;
        AddedAtUtc = addedAtUtc;
    }
}