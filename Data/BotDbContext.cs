using FbkiBot.Configuration;
using Microsoft.EntityFrameworkCore;

namespace FbkiBot.Data;

/// <summary>
/// Контекст БД чатбота
/// </summary>
public class BotDbContext : DbContext
{
    private readonly DatabaseSettings _dbSettings;

    /// <summary>
    /// Действия при создании БД
    /// </summary>
    /// <param name="dbSettings">Настройки Sqlite</param>
    public BotDbContext(DatabaseSettings dbSettings)
    {
        _dbSettings = dbSettings;

        // Убеждаемся что файл БД существует
        Database.EnsureCreated();
    }

    /// <summary>
    /// Действиия при конфигурации БД
    /// </summary>
    /// <param name="optionsBuilder">Билдер БД</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Используем Sqlite и передаем путь до файла БД
        optionsBuilder.UseSqlite(_dbSettings.SqliteConnection);
    }
}