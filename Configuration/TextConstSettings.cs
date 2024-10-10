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
    public required string RmSuccessMessage { get; set; }

    /// <summary>
    /// Сообщение, которое отправляется при успешном выполнении команды /ls
    /// </summary>
    public required string LsSuccessMessage { get; set; }

    /// <summary>
    /// Сообщение, которое отправляется когда монтируется личный чат с ботом
    /// </summary>
    public required string MountIsPersonalChatMessage { get; set; }

    /// <summary>
    /// Сообщение, которое отправляется когда пользователь не задал название монтирования
    /// </summary>
    public required string MountNoNameMessage { get; set; }

    /// <summary>
    /// Сообщение, которое отправляется когда монитрование с таким названием у пользователя уже сществует
    /// </summary>
    public required string MountNameTakenMessage { get; set; }

    /// <summary>
    /// Сообщение, которое отправляется когда монитрование этого чата уже было произведено у пользователя
    /// </summary>
    public required string MountIsExistsMessage { get; set; }

    /// <summary>
    /// Сообщение, которое отправляется при успешном выполнении команды /mount
    /// </summary>
    public required string MountSuccessMessage { get; set; }

    /// <summary>
    /// Сообщение, которое отправляется когда этот чат не примонтирован
    /// </summary>
    public required string RmMountNotFoundMessage { get; set; }


    /// <summary>
    /// Сообщение, которое отправляется при успешном выполнении команды /rmmount
    /// </summary>
    public required string RmMountSuccessMessage { get; set; }
}