namespace FbkiBot.Models;

/// <summary>
///     Найденное сообщениеб включается в себя сохраненное сообщение и имя монтирования по котором оно было найдено, нужно
///     для правильного вывода сообщений
/// </summary>
public class FoundMessage
{
    /// <summary>
    ///     Создать модель сохраненного сообщения
    /// </summary>
    /// <param name="mountName">Название</param>
    /// <param name="savedMessage">Сохранненное сообщение</param>
    public FoundMessage(SavedMessage savedMessage, string? mountName)
    {
        Message = savedMessage;
        MountName = mountName;
    }

    /// <summary>
    ///     ID в БД
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     Название монтирования
    /// </summary>
    public string? MountName { get; set; }

    /// <summary>
    ///     Сообщение которое найдено по монтированию
    /// </summary>
    public SavedMessage Message { get; set; }
}