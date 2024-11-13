namespace FbkiBot.Configuration;

/// <summary>
///     Настройки, специфичные для Telegram и Telegram-бота
/// </summary>
public class TelegramSettings
{
    /// <summary>
    ///     Токен, по которому работает бот
    /// </summary>
    public required string BotToken { get; set; }
}