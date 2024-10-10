using FbkiBot.Models;
using Microsoft.EntityFrameworkCore;

namespace FbkiBot.Data;

/// <summary>
/// Контекст БД чатбота
/// </summary>
public class BotDbContext : DbContext
{
    /// <summary>
    /// Сообщения, сохраненные командой /save
    /// </summary>
    public DbSet<SavedMessage> SavedMessages { get; set; }

    public DbSet<UserMount> UserMounts { get; set; }

    public async Task<UserMount?> FindUserMountAsync(string name, long userId, CancellationToken cancellationToken) => await UserMounts.SingleOrDefaultAsync(msg => msg.UserId == userId && EF.Functions.Like(msg.Name, name), cancellationToken: cancellationToken);

    public async Task<SavedMessage?> FindSavedMessageAsync(string name, long chatId, CancellationToken cancellationToken) => await SavedMessages.SingleOrDefaultAsync(msg => msg.ChatId == chatId && EF.Functions.Like(msg.Name, name), cancellationToken: cancellationToken);

    /// <summary>
    /// Действия при создании БД
    /// </summary>
    public BotDbContext(DbContextOptions<BotDbContext> options) : base(options)
    {
        // Убеждаемся что файл БД существует
        Database.EnsureCreated();
    }
}