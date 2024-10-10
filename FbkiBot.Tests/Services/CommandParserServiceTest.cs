using FbkiBot.Services;
using Telegram.Bot.Types;

namespace FbkiBot.Tests.Services;

public class CommandParserServiceTest
{
    [Fact]
    public void CanParseNonCommandMessage()
    {
        var parser = new CommandParserService();
        var message = new Message() { Text = "Hello, World!" };

        var context = parser.BuildContext(message);

        Assert.Equal(2, context.Arguments.Length);
        Assert.Null(context.Command);
    }

    [Fact]
    public void CanParseSlashCommandMessageNoArgs()
    {
        var parser = new CommandParserService();
        var message = new Message() { Text = "/start" };

        var context = parser.BuildContext(message);

        Assert.Equal("/start", context.Command);
        Assert.Empty(context.Arguments);
    }

    [Fact]
    public void CanParseSlashCommandMessageArgs()
    {
        var parser = new CommandParserService();
        var message = new Message() { Text = "/echo Hello, World!" };

        var context = parser.BuildContext(message);

        Assert.Equal("/echo", context.Command);
        Assert.Equal(["Hello,", "World!"], context.Arguments.AsSpan());
    }

    [Fact]
    public void CanParseEmptyMessage()
    {
        var parser = new CommandParserService();
        var message = new Message() { Text = "" };

        var context = parser.BuildContext(message);

        Assert.Null(context.Command);
        Assert.Equal([], context.Arguments.AsSpan());
    }

    [Fact]
    public void CanParseCyrillicNonCommandMessage()
    {
        var parser = new CommandParserService();
        var message = new Message() { Text = "Привет, мир!" };

        var context = parser.BuildContext(message);

        Assert.Null(context.Command);
        Assert.Equal(["Привет,", "мир!"], context.Arguments.AsSpan());
    }

    [Fact]
    public void CanParseCyrillicSlashCommandMessage()
    {
        var parser = new CommandParserService();
        var message = new Message() { Text = "/привет мир" };

        var context = parser.BuildContext(message);

        Assert.Equal("/привет", context.Command);
        Assert.Equal(["мир"], context.Arguments.AsSpan());
    }

    [Fact]
    public void RemovesEmptyArgumentsFromMessagesWithDoubleSpaces()
    {
        var parser = new CommandParserService();
        var message = new Message() { Text = "/echo  test" };

        var context = parser.BuildContext(message);

        Assert.Equal("/echo", context.Command);
        Assert.Equal(["test"], context.Arguments.AsSpan());
    }
}