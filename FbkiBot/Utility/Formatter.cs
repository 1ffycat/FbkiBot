using Telegram.Bot.Types.Enums;

namespace FbkiBot.Utility;

/// <summary>
/// Класс для форматирования строк в сообщениях
/// </summary>
public static class Formatter
{
    /// <summary>
    /// Сформатировать текст курсивом
    /// </summary>
    /// <param name="text">Текст</param>
    /// <param name="parseMode">Режим форматирования</param>
    /// <returns>Строка с добавленными форматирующими символами для соответствующего режима форматирования</returns>
    public static string Italic(string text, ParseMode parseMode = ParseMode.MarkdownV2)
    {
        return Format(text, "_", "i", parseMode);
    }

    /// <summary>
    /// Сформатировать текст жирным текстом
    /// </summary>
    /// <param name="text">Текст</param>
    /// <param name="parseMode">Режим форматирования</param>
    /// <returns>Строка с добавленными форматирующими символами для соответствующего режима форматирования</returns>
    public static string Bold(string text, ParseMode parseMode = ParseMode.MarkdownV2)
    {
        return Format(text, "*", "b", parseMode);
    }

    /// <summary>
    /// Сформатировать текст подчеркнутым
    /// </summary>
    /// <param name="text">Текст</param>
    /// <param name="parseMode">Режим форматирования</param>
    /// <returns>Строка с добавленными форматирующими символами для соответствующего режима форматирования</returns>
    public static string Underline(string text, ParseMode parseMode = ParseMode.MarkdownV2)
    {
        return Format(text, "__", "u", parseMode);
    }

    /// <summary>
    /// Сформатировать текст зачеркнутым
    /// </summary>
    /// <param name="text">Текст</param>
    /// <param name="parseMode">Режим форматирования</param>
    /// <returns>Строка с добавленными форматирующими символами для соответствующего режима форматирования</returns>
    public static string Strikethrough(string text, ParseMode parseMode = ParseMode.MarkdownV2)
    {
        return Format(text, "~", "s", parseMode);
    }

    /// <summary>
    /// Сформатировать текст как спойлер
    /// </summary>
    /// <param name="text">Текст</param>
    /// <param name="parseMode">Режим форматирования</param>
    /// <returns>Строка с добавленными форматирующими символами для соответствующего режима форматирования</returns>
    public static string Spoiler(string text, ParseMode parseMode = ParseMode.MarkdownV2)
    {
        return Format(text, "||", """<span class="tg-spoiler">""", "</span>", parseMode);
    }

    /// <summary>
    /// Сформатировать текст моноширотным
    /// </summary>
    /// <remarks>Позволяет копировать по клику или тапу</remarks>
    /// <remarks>То же самое, что `текст`</remarks>
    /// <param name="text">Текст</param>
    /// <param name="parseMode">Режим форматирования</param>
    /// <returns>Строка с добавленными форматирующими символами для соответствующего режима форматирования</returns>
    public static string Monospace(string text, ParseMode parseMode = ParseMode.MarkdownV2)
    {
        return Format(text, "`", "pre", parseMode);
    }

    /// <summary>
    /// Сформатировать текст как однострочный код
    /// </summary>
    /// <remarks>Результат такой же, как от Monospace, но в HTML используется тег code</remarks>
    /// <param name="text">Текст</param>
    /// <param name="parseMode">Режим форматирования</param>
    /// <returns>Строка с добавленными форматирующими символами для соответствующего режима форматирования</returns>
    public static string Code(string text, ParseMode parseMode = ParseMode.MarkdownV2)
    {
        return Format(text, "`", "code", parseMode);
    }

    /// <summary>
    /// Сформатировать код как многострочный код
    /// </summary>
    /// <param name="text">Текст</param>
    /// <param name="language">Язык кода</param>
    /// <param name="parseMode">Режим форматирования</param>
    /// <returns>Строка с добавленными форматирующими символами для соответствующего режима форматирования</returns>
    public static string MultilineCode(string text, string language = "", ParseMode parseMode = ParseMode.MarkdownV2)
    {
        return Format(text, $"```{language}\n", "\n```", $"""<pre><code class="{language}">""", "</code></pre>",
            parseMode);
    }

    /// <summary>
    /// Сформатировать текст как цитату
    /// </summary>
    /// <param name="text">Текст</param>
    /// <param name="parseMode">Режим форматирования</param>
    /// <returns>Строка с добавленными форматирующими символами для соответствующего режима форматирования</returns>
    public static string Quote(string text, ParseMode parseMode = ParseMode.MarkdownV2)
    {
        return Format(parseMode is ParseMode.MarkdownV2 ? '>' + text.Replace("\n", "\n>") : text,
            "", "blockquote", parseMode);
    }

    /// <summary>
    /// Сформатировать текст как разворачиваемую цитату
    /// </summary>
    /// <param name="text">Текст</param>
    /// <param name="parseMode">Режим форматирования</param>
    /// <returns>Строка с добавленными форматирующими символами для соответствующего режима форматирования</returns>
    public static string ExpandableQuote(string text, ParseMode parseMode = ParseMode.MarkdownV2)
    {
        return Format(parseMode is ParseMode.MarkdownV2 ? '>' + text.Replace("\n", "\n>") : text, "**", "||",
            "<blockquote expandable>", "</blockquote>", parseMode);
    }

    /// <summary>
    /// Сформатировать код в зависимости от режима форматирования
    /// </summary>
    /// <param name="text">Текст</param>
    /// <param name="markdownSymbol">Символ, в который обернуть текст в MarkdownV2</param>
    /// <param name="htmlOpeningTag">Открывающий тег в HTML</param>
    /// <param name="htmlClosingTag">Закрывающий тег в HTML</param>
    /// <param name="parseMode">Режим форматирования</param>
    /// <remarks>В отличие от перегрузки с htmlTagName, тут нужно добавлять треугольные скобки</remarks>
    /// <returns>Строка с добавленными форматирующими символами для соответствующего режима форматирования</returns>
    private static string Format(string text, string markdownSymbol, string htmlOpeningTag, string htmlClosingTag,
        ParseMode parseMode)
    {
        return Format(text, markdownSymbol, markdownSymbol, htmlOpeningTag, htmlClosingTag, parseMode);
    }

    /// <summary>
    /// Сформатировать код в зависимости от режима форматирования
    /// </summary>
    /// <param name="text">Текст</param>
    /// <param name="markdownOpeningSymbol">Открывающий символ в MarkdownV2</param>
    /// <param name="markdownClosingSymbol">Закрывающий символ в MarkdownV2</param>
    /// <param name="htmlOpeningTag">Открывающий тег в HTML</param>
    /// <param name="htmlClosingTag">Закрывающий тег в HTML</param>
    /// <param name="parseMode">Режим форматирования</param>
    /// <remarks>В отличие от перегрузки с htmlTagName, тут нужно добавлять треугольные скобки</remarks>
    /// <returns>Строка с добавленными форматирующими символами для соответствующего режима форматирования</returns>
    private static string Format(string text, string markdownOpeningSymbol, string markdownClosingSymbol,
        string htmlOpeningTag, string htmlClosingTag, ParseMode parseMode)
    {
        return parseMode switch
        {
            ParseMode.MarkdownV2 => $"{markdownOpeningSymbol}{text}{markdownClosingSymbol}",
            ParseMode.Html => $"{htmlOpeningTag}{text}{htmlClosingTag}",
            ParseMode.Markdown => throw new NotSupportedException(
                "Markdown formatting is obsolete, please use MarkdownV2."),
            _ => text
        };
    }

    /// <summary>
    /// Сформатировать код в зависимости от режима форматирования
    /// </summary>
    /// <param name="text">Текст</param>
    /// <param name="markdownSymbol">Символ, в который обернуть в MarkdownV2</param>
    /// <param name="htmlTagName">Название HTML-тега без треугольных скобок</param>
    /// <param name="parseMode">Режим форматирования</param>
    /// <remarks>В отличие от перегрузки с открывающими/закрывающими HTML-тегами, тут НЕ нужно добавлять треугольные скобки</remarks>
    /// <returns>Строка с добавленными форматирующими символами для соответствующего режима форматирования</returns>
    private static string Format(string text, string markdownSymbol, string htmlTagName,
        ParseMode parseMode)
    {
        return Format(text, markdownSymbol, $"<{htmlTagName}>", $"</{htmlTagName}>", parseMode);
    }
}