using System.Reflection;
using System.Text;
using FbkiBot.Attributes;
using FbkiBot.Models;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace FbkiBot.Commands;

[BotCommand("/help", "Выводит список всех доступных команд")]
public class HelpCommand(ILogger<HelpCommand> logger, Func<IEnumerable<IChatCommand>> commandFactory) : IChatCommand
{
    public bool CanExecute(CommandContext context)
    {
        return context.Command?.Equals("/help", StringComparison.OrdinalIgnoreCase) ?? false;
    }

    public async Task ExecuteAsync(ITelegramBotClient botClient, CommandContext context,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Received /help command. Building help message.");
        var helpBuilder =
            new StringBuilder(); // StringBuilder работает быстрее конкатенации и минимизирует аллокацию строк в оперативке

        foreach (var command in commandFactory()) // Получаем все добавленные в DI команды и проходимся по ним
        {
            // Получаем атрибут BotCommand, содержащий название, описание и пример использования команды
            var attr = command.GetType().GetCustomAttribute<BotCommandAttribute>();

            // Если такого атрибута не найдено - скипаем команду. В отправленном юзеру сообщении этой команды не будет
            if (attr is null) continue;

            // Добавляем строку в формате: "название: описание (использование)" либо "название: описание" взависимости от наличия примера использования
            helpBuilder.AppendLine(
                $"{attr.Name}: {attr.Description} {(attr.Usage is not null ? $"({attr.Usage})" : "")}");
        }

        // Отправляем юзеру собранное сообщение
        await botClient.SendTextMessageAsync(context.Message.Chat.Id, helpBuilder.ToString(),
            cancellationToken: cancellationToken);
    }
}