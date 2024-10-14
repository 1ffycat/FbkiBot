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

// Добавляем файл с текстовыми константами
builder.Configuration.AddJsonFile("textconsts.json");

// Парсим настройки
builder.AddSettings<TelegramSettings>("Telegram");
builder.AddSettings<TextConstSettings>("TextConsts");
builder.AddSettings<MessageSavingSettings>("MessageSaving");

// Добавляем БД в DI контейнер
builder.Services.AddDbContext<BotDbContext>(conf =>
{
    conf.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
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
builder.AddCommand<UmountCommand>();
builder.AddCommand<MountsCommand>();

// Обходим круговую зависимость в HelpCommand. Костыли еще никогда не были так лаконичны
builder.Services.AddTransient<Func<IEnumerable<IChatCommand>>>(sp =>
    () => sp.GetServices<IChatCommand>());

// Собираем все что понадобавляли
var app = builder.Build();

// Запускаем автоботов в бой
app.Run();