using FbkiBot.Commands;
using FbkiBot.Configuration;
using FbkiBot.Data;
using FbkiBot.Services;
using FbkiBot.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

// Парсим настройки
builder.AddSettings<TelegramSettings>("Telegram");
builder.AddSettings<TextConstSettings>("TextConsts");
builder.AddSettings<MessageSavingSettings>("Message saving");

// Добавляем БД в DI контейнер
builder.Services.AddDbContext<BotDbContext>(conf =>
{
    conf.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
});

// Добавляем сервисы в DI контейнер
builder.Services.AddSingleton<IBotService, TelegramBotService>();
builder.Services.AddHostedService<BotHostedService>();
builder.Services.AddSingleton<CommandParserService>();

// Добавляем команды в DI контейнер
builder.AddCommand<StartCommand>();
builder.AddCommand<HelpCommand>();
builder.AddCommand<SaveCommand>();
builder.AddCommand<CatCommand>();
builder.AddCommand<RmCommand>();
builder.AddCommand<LsCommand>();
builder.AddCommand<MountCommand>();

// Обходим круговую зависимость в HelpCommand. Костыли еще никогда не были так лаконичны
builder.Services.AddTransient<Func<IEnumerable<IChatCommand>>>(sp =>
    () => sp.GetServices<IChatCommand>());

// Собираем все что понадобавляли
var app = builder.Build();

// Запускаем автоботов в бой
app.Run();