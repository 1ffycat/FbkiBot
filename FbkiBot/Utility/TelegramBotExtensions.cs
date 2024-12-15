using FbkiBot.Resources;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FbkiBot.Utility;

public static class TelegramBotExtensions
{
    /// <summary>
    /// Попробовать отправить сообщение в ЛС пользователю. Если не получилось - отправить его в чат с предупреждением
    /// </summary>
    /// <param name="bot">Клиент бота</param>
    /// <param name="user">Пользователь, которому отправить сообщение</param>
    /// <param name="text">Текст сообщение</param>
    /// <param name="chat">Чат, в который отправить сообщение при закрытых ЛС</param>
    /// <param name="cancellationToken">Токен для отмены действия</param>
    public static async Task TrySendMessageOrNotify(this ITelegramBotClient bot, User user, string text, Chat chat,
        CancellationToken cancellationToken = default)
    {
        try // Пробуем отправить сообщение в ЛС
        {
            await bot.SendMessage(user.Id, text, cancellationToken: cancellationToken);
        }
        catch // Если не получилось - отправляем в чат с предупреждением
        {
            await bot.SendMessage(chat!.Id,
                $"{text}\n\n{Formatter.Italic(SystemStrings.CannotSendPrivateMessage, ParseMode.Html)}",
                cancellationToken: cancellationToken, parseMode: ParseMode.Html);
        }
    }
}