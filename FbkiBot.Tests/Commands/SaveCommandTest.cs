using FbkiBot.Commands;
using FbkiBot.Configuration;
using FbkiBot.Data;
using FbkiBot.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FbkiBot.Tests.Commands;

public class SaveCommandTest
{
    private BotDbContext MakeInMemoryDb()
    {
        var con = new SqliteConnection("Filename=:memory:");
        con.Open();
        return new BotDbContext(new DbContextOptionsBuilder<BotDbContext>().UseSqlite(con).Options);
    }

    private (BotDbContext db, ILogger<SaveCommand> logger, IOptions<TextConstSettings> textConsts, CommandContext context, ITelegramBotClient botClient) BuildEnvironment(string command, string[] args)
    {
        string[] tokens = [command, .. args];
        var msg = new Message() { Text = string.Join(' ', tokens), ReplyToMessage = new Message() { MessageId = 0 }, From = new() { Id = 1, FirstName = "A", LastName = "B", Username = "C" }, Chat = new() { Id = 2 } };
        var ctx = new CommandContext(msg, command, args);

        return (MakeInMemoryDb(), Substitute.For<ILogger<SaveCommand>>(), Options.Create(Substitute.For<TextConstSettings>()), ctx, Substitute.For<ITelegramBotClient>());
    }

    [Fact]
    public void TriggersOnCorrectCommand()
    {
        var env = BuildEnvironment("/save", ["test"]);
        var command = new SaveCommand(env.db, env.logger, env.textConsts);

        Assert.True(command.CanExecute(env.context));
    }

    [Fact]
    public void DoesntTriggerOnWrongCommand()
    {
        var env = BuildEnvironment("/wrong", ["test"]);
        var command = new SaveCommand(env.db, env.logger, env.textConsts);

        Assert.False(command.CanExecute(env.context));
    }

    [Fact]
    public async Task HandlesEmptyArgs()
    {
        var env = BuildEnvironment("/wrong", []);
        var command = new SaveCommand(env.db, env.logger, env.textConsts);

        await command.ExecuteAsync(env.botClient, env.context, CancellationToken.None);

        Assert.Empty(env.db.SavedMessages);
    }

    [Fact]
    public async Task HandlesNoReply()
    {
        var env = BuildEnvironment("/save", ["test"]);
        var command = new SaveCommand(env.db, env.logger, env.textConsts);
        env.context.Message.ReplyToMessage = null;

        await command.ExecuteAsync(env.botClient, env.context, CancellationToken.None);

        Assert.Empty(env.db.SavedMessages);
    }

    [Fact]
    public async Task HandlesExistingNames()
    {
        var env = BuildEnvironment("/save", ["test"]);
        var command = new SaveCommand(env.db, env.logger, env.textConsts);
        await env.db.SavedMessages.AddAsync(new SavedMessage("test", 69, 1337, new()));
        await env.db.SaveChangesAsync();

        await command.ExecuteAsync(env.botClient, env.context, CancellationToken.None);

        Assert.Equal(69, env.db.SavedMessages.First().MessageId);
        Assert.Equal(1337, env.db.SavedMessages.First().ChatId);
    }

    [Fact]
    public async Task SavesMessageAsync()
    {
        var env = BuildEnvironment("/save", ["test"]);
        var command = new SaveCommand(env.db, env.logger, env.textConsts);

        await command.ExecuteAsync(env.botClient, env.context, CancellationToken.None);

        Assert.NotEmpty(env.db.SavedMessages);
        var sm = env.db.SavedMessages.First();
        Assert.Equal(0, sm.MessageId);
        Assert.Equal(1, sm.AddedById);
        Assert.Equal(2, sm.ChatId);
        Assert.Equal("A B", sm.AddedByName);
        Assert.Equal("C", sm.AddedByUsername);
    }
}