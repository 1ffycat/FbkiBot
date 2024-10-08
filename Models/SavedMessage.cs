namespace FbkiBot.Models;

/// <summary>
/// Сохраненное сообщение
/// </summary>
public class SavedMessage
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
    /// ID пользователя, сохранившего сообщение
    /// </summary>
    public long AddedById { get; set; }

    /// <summary>
    /// ID чата, в котором сохранено сообщение
    /// </summary>
    public long ChatId { get; set; }

    /// <summary>
    /// Название сохраненного сообщения
    /// </summary>
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
    public SavedMessage(string name, int messageId, long chatId, long addedById)
    {
        Name = name;
        MessageId = messageId;
        ChatId = chatId;
        AddedById = addedById;

        // Ставим дату сохранения на текущую по UTC
        AddedAtUtc = DateTime.UtcNow;
    }
}