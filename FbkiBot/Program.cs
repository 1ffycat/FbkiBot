using System.Reflection;
using FbkiBot.Commands;
using FbkiBot.Configuration;
using FbkiBot.Data;
using FbkiBot.Services;
using FbkiBot.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Types;

var builder = Host.CreateApplicationBuilder(args);

// Добавляем файл с текстовыми константами
builder.Configuration.AddDefaultsJsonFile("textconsts.json", optional: true);
builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), true);

// Парсим настройки
builder.Services.AddSettings<TelegramSettings>(builder.Configuration.GetSection("Telegram"));
builder.Services.AddSettings<TextConstSettings>(builder.Configuration.GetSection("TextConsts"));
builder.Services.AddSettings<MessageSavingSettings>(builder.Configuration.GetSection("MessageSaving"));

// Добавляем временную in-memory БД
if (builder.Environment.IsDevelopment())
    builder.Services.AddDbContext<BotDbContext>(conf =>
        {
            conf.UseInMemoryDatabase("tmpdb");
        }, ServiceLifetime.Singleton  // Добавляем как singleton для сохранения данных в БД между запросами
    );
// Добавляем постоянную PostgreSQL БД
else builder.Services.AddDbContext<BotDbContext>(conf =>
{
    conf.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
});

builder.AddMiddlewarePipeline();

// Добавляем сервисы в DI контейнер
builder.Services.AddHostedService<TelegramBotService>();
builder.Services.AddSingleton<CommandParserService>();

// Добавляем команды в DI контейнер
builder.Services.AddCommand<StartCommand>();
builder.Services.AddCommand<HelpCommand>();
builder.Services.AddCommand<SaveCommand>();
builder.Services.AddCommand<CatCommand>();
builder.Services.AddCommand<RmCommand>();
builder.Services.AddCommand<LsCommand>();
builder.Services.AddCommand<MountCommand>();
builder.Services.AddCommand<UmountCommand>();
builder.Services.AddCommand<MountsCommand>();

// Обходим круговую зависимость в HelpCommand. Костыли еще никогда не были так лаконичны
builder.Services.AddTransient<Func<IEnumerable<IChatCommand>>>(sp =>
    sp.GetServices<IChatCommand>);

// Собираем все что понадобавляли
var app = builder.Build();

// Добавляем ПО промежуточного слоя
app.UseErrorHandler();  // Обработчик ошибок
app.UseUpdateLogger();  // Логгирование полученных событий
if (builder.Environment.IsDevelopment()) app.UseRequestTimer();  // Замер времени обработки запроса
app.UseTextCommands();  // Обработчик текстовых команд

// Пример анонимного ПО промежуточного слоя - повторяет все полученные стикеры в чате
app.Use(async (context, next) =>
{
    // Если не стикер - пропускаем
    if (context.Update.Message?.Sticker is not { } sticker)
    {
        await next(context);
        return;
    }

    // Отправляем тот же стикер в чат
    await context.Client.SendStickerAsync(context.Update.Message.Chat.Id, InputFile.FromFileId(sticker.FileId));

    // Продолжаем выполнение
    await next(context);
});

// Запускаем автоботов в бой
app.Run();