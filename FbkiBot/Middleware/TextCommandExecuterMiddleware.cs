using FbkiBot.Commands;
using FbkiBot.Models;
using FbkiBot.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FbkiBot.Middleware;

/// <summary>
///     ПО промежуточного слоя, находящее и выполняющее подходящие текстовые команды
/// </summary>
public class TextCommandExecuterMiddleware : IBotMiddleware
{
    /// <summary>
    ///     Найти и выполнить все подходящие текстовые команды
    /// </summary>
    public async Task InvokeAsync(UpdateContext context, BotMiddlewareDelegate next)
    {
        // Если не текстовое сообщение - пропускаем
        if (context.Update.Message?.Text is null)
        {
            await next(context);
            return;
        }

        await using var scope = context.Services.CreateAsyncScope();

        var logger = scope.ServiceProvider.GetRequiredService<ILogger<TextCommandExecuterMiddleware>>();

        // Получаем список всех команд
        var commands = scope.ServiceProvider.GetServices<IChatCommand>().ToList();

        logger.LogDebug("Found {count} commands.", commands.Count);

        // Строим контекст команды
        var cmdParser = scope.ServiceProvider.GetRequiredService<CommandParserService>();
        var ctx = cmdParser.BuildContext(context.Update.Message!);

        // Список всех подходящих команд
        var matchingCommands = commands.Where(command => command.CanExecute(ctx)).ToList();

        logger.LogDebug("{count} commands match", matchingCommands.Count);

        // Выполняем все подходящие команды
        foreach (var command in matchingCommands)
        {
            logger.LogDebug("Executing command: {name}", command.GetType().Name);
            await command.ExecuteAsync(context.Client, ctx, context.CancellationToken);
            logger.LogDebug("Done executing command {name}", command.GetType().Name);
        }

        // Продолжаем выполнение пайплайна
        await next(context);
    }
}