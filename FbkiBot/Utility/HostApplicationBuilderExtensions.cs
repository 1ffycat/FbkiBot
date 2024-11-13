using FbkiBot.Commands;
using FbkiBot.Middleware;
using FbkiBot.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FbkiBot.Utility;

/// <summary>
/// Расширения для билдера приложения
/// </summary>
public static class HostApplicationBuilderExtensions
{
    public static IHostApplicationBuilder AddMiddlewarePipeline(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<BotMiddlewarePipeline>();

        return builder;
    }
}