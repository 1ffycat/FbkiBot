using Microsoft.EntityFrameworkCore;

namespace FbkiBot.Data;

/// <summary>
/// Контекст БД чатбота
/// </summary>
public class BotDbContext : DbContext
{
    /// <summary>
    /// Действия при создании БД
    /// </summary>
    public BotDbContext(DbContextOptions<BotDbContext> options) : base(options)
    {
        // Убеждаемся что файл БД существует
        Database.EnsureCreated();
    }
}