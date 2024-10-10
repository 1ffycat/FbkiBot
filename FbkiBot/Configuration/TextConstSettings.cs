namespace FbkiBot.Configuration;

/// <summary>
/// Модель настроек текстовых констант
/// </summary>
public class TextConstSettings
{
    /// <summary>
    /// Приветственное сообщение, выводимое в ответ на команду /start
    /// </summary>
    public required string WelcomeMessage { get; set; }

    /// <summary>
    /// Сообщение, которое отправляется когда пользователь не задал название для сохраненного сообщения
    /// </summary>
    public required string SaveNoNameProvidedMessage { get; set; }

    /// <summary>
    /// Сообщение, которое отправляется когда пользователь не отметил сообщение для сохранения
    /// </summary>
    public required string SaveNoReplyMessage { get; set; }

    /// <summary>
    /// Сообщение, которое отправляется когда сообщение успешно сохранено
    /// </summary>
    public required string SaveMessageSavedMessage { get; set; }

    /// <summary>
    /// Сообщение, которое отправляется когда сохраненное сообщение с таким названием уже существует
    /// </summary>
    public required string SaveNameTakenMessage { get; set; }

    /// <summary>
    /// Сообщение, которое отправляется когда команда /cat не находит сообщение по данному названию
    /// </summary>
    public required string CatNotFoundMessage { get; set; }

    /// <summary>
    /// Сообщение, которое отправляется когда команда /cat успешно находит сообщение
    /// </summary>
    public required string CatFoundMessage { get; set; }

    /// <summary>
    /// Сообщение, которое отправляется когда пользователь пытается удалить чужое сохраненное сообщение
    /// </summary>
    public required string RmNotAuthorMessage { get; set; }

    /// <summary>
    /// Сообщение, которое отправляется когда сохраненное сообщение было успешно удалено
    /// </summary>
    public required string RmSuccess { get; set; }

    /// <summary>
    /// Сообщение, которое отправляется при успешном выполнении команды /ls
    /// </summary>
    public required string LsSuccess { get; set; }
}