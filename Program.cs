using FbkiBot.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<TelegramSettings>(builder.Configuration.GetSection("Telegram"));

var app = builder.Build();
app.Run();