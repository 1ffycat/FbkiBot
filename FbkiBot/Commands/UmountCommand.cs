using FbkiBot.Attributes;
using FbkiBot.Configuration;
using FbkiBot.Data;
using FbkiBot.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FbkiBot.Commands;

[BotCommand("/umount", "Удаляет mount", "/umount")]
public class UmountCommand(BotDbContext db, ILogger<UmountCommand> logger, IOptions<TextConstSettings> textConsts) : IChatCommand
{
    public bool CanExecute(CommandContext context) => context.Command?.Equals("/umount", StringComparison.OrdinalIgnoreCase) ?? false;

    public async Task ExecuteAsync(ITelegramBotClient botClient, CommandContext context, CancellationToken cancellationToken)
    {
        logger.LogDebug("Processing umount command");

        // Ищем монтирование чата по пользователю, который написал команду и Id чата в котором была написана команда
        var mount = db.UserMounts.FirstOrDefault(mnt => mnt.UserId == context.Message.From!.Id && mnt.ChatId == context.Message.Chat.Id);

        // Если такое понтирование не найдено сообщаем об этом пользователю 
        if (mount is null)
        {
            await botClient.SendTextMessageAsync(context.Message.Chat.Id, textConsts.Value.UmountNotFoundMessage, cancellationToken: cancellationToken);
            return;
        }

        // Удаляем монтирование и сохраняем изменения в БД
        db.UserMounts.Remove(mount);
        await db.SaveChangesAsync(cancellationToken: cancellationToken);

        logger.LogDebug("/umount - success");
        await botClient.SendTextMessageAsync(context.Message.Chat.Id, textConsts.Value.UmountSuccessMessage, cancellationToken: cancellationToken);
    }
}