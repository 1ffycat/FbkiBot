using FbkiBot;
using FbkiBot.Commands;
using FbkiBot.Configuration;
using FbkiBot.Data;
using FbkiBot.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

// Парсим настройки
builder.Services.Configure<TelegramSettings>(builder.Configuration.GetSection("Telegram"));
builder.Services.Configure<TextConstSettings>(builder.Configuration.GetSection("TextConsts"));

// Добавляем БД в DI контейнер
builder.Services.AddDbContext<BotDbContext>(conf =>
{
    conf.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
});

// Добавляем сервисы в DI контейнер
builder.Services.AddSingleton<IBotService, TelegramBotService>();
builder.Services.AddHostedService<BotHostedService>();

// Добавляем команды в DI контейнер
builder.Services.AddTransient<IChatCommand, StartCommand>();
builder.Services.AddTransient<IChatCommand, HelpCommand>();

// Обходим круговую зависимость в HelpCommand. Костыли еще никогда не были так лаконичны
builder.Services.AddTransient<Func<IEnumerable<IChatCommand>>>(sp =>
    () => sp.GetServices<IChatCommand>());

// Собираем все что понадобавляли
var app = builder.Build();

// Запускаем автоботов в бой
app.Run();