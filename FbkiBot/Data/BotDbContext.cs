using FbkiBot.Models;
using Microsoft.EntityFrameworkCore;

namespace FbkiBot.Data;

/// <summary>
///     Контекст БД чатбота
/// </summary>
public sealed class BotDbContext : DbContext
{
    /// <summary>
    ///     Действия при создании БД
    /// </summary>
    public BotDbContext(DbContextOptions<BotDbContext> options) : base(options)
    {
        // Убеждаемся что файл БД существует
        //Database.EnsureCreated();
        if (!Database.IsInMemory())
        {
            Database.Migrate();
        }
        else
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }
    }

    /// <summary>
    ///     Сообщения, сохраненные командой /save
    /// </summary>
    public DbSet<SavedMessage> SavedMessages { get; set; }

    /// <summary>
    ///     Монтирование, добавленное командой /mount
    /// </summary>
    public DbSet<UserMount> UserMounts { get; set; }

    public async Task<SavedMessage?> FindSavedMessageAsync(string name, long chatId,
        CancellationToken cancellationToken)
    {
        return await SavedMessages.SingleOrDefaultAsync(
            msg => msg.ChatId == chatId && EF.Functions.Like(msg.Name, name), cancellationToken);
    }
}