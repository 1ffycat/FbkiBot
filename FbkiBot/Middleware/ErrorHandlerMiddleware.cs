using System.Text;
using FbkiBot.Models;
using FbkiBot.Resources;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace FbkiBot.Middleware;

/// <summary>
///     ПО промежуточного слоя, отправляющее сообщения о произошедших ошибках пользователю
/// </summary>
public class ErrorHandlerMiddleware : IBotMiddleware
{
    /// <summary>
    ///     Выполнить middleware
    /// </summary>
    public async Task InvokeAsync(UpdateContext context, BotMiddlewareDelegate next)
    {
        var config = context.Services.GetRequiredService<IConfiguration>();

        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            if (context.Update.Message is null) return;

            var sb = new StringBuilder();

            sb.Append(SystemStrings.ErrorHandling_UnexpectedError);

            sb.Append(config.GetValue<bool>("ErrorHandling:SendTraces")
                    ? ex.ToString() // Вся ошибка, включая execution trace
                    : $"{ex.GetType().Name}: {ex.Message}" // Только тип и сообщение ошибки
            );

            // Отправить сообщение об ошибке пользователю, отправившему сообщение, повлекшее появление ошибки
            await context.Client.SendTextMessageAsync(context.Update.Message.Chat.Id, sb.ToString(),
                cancellationToken: context.CancellationToken, replyToMessageId: context.Update.Message.MessageId,
                parseMode: ParseMode.Html);
        }
    }
}