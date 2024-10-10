namespace FbkiBot.Configuration;

/// <summary>
/// Модель настроек сохранения сообщений
/// </summary>
public class MessageSavingSettings
{
    /// <summary>
    /// Только ли автор может удалять сохраненные сообщения
    /// </summary>
    public bool CanOnlyBeRemovedByAuthor { get; set; } = false;
}