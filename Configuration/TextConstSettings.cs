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
}