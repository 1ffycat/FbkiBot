namespace FbkiBot.Models;

/// <summary>
/// Найденное сообщение
/// </summary>
public class FoundMessage
{
    /// <summary>
    /// ID в БД
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название монтирования
    /// </summary>
    public string? MountName { get; set; }

    /// <summary>
    /// Сообщение
    /// </summary>
    public SavedMessage Message { get; set; }

    /// <summary>
    /// Создать модель сохраненного сообщения
    /// </summary>
    /// <param name="mountName">Название</param>
    /// <param name="savedMessage">Сохранненное сообщение</param>
    public FoundMessage(SavedMessage savedMessage, string? mountName)
    {
        Message = savedMessage;
        MountName = mountName;
    }
}