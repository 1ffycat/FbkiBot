namespace FbkiBot.Configuration;

/// <summary>
/// Настройки базы данных Sqlite
/// </summary>
public class DatabaseSettings
{
    /// <summary>
    /// Строка подключения к БД. В случае Sqlite - путь до файла в формате "Data Source=<path>"
    /// </summary>
    public required string SqliteConnection { get; set; }
}
