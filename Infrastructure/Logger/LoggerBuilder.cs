using Infrastructure.Bot;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using static System.Int64;

namespace Infrastructure.Logger;

public static class LoggerBuilder
{
    public static void BuildLogger(this WebApplicationBuilder builder)
    {
        var (token, chatId) = GetTokenAndChat(builder);

        builder.Logging.ClearProviders();
        var logger = new LoggerConfiguration().WriteTo
            .Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
            //.WriteTo.File("log.txt")
#if !DEBUG
            .WriteTo.TelegramSink(token, chatId)
#endif
            .CreateLogger();
        builder.Logging.AddSerilog(logger);
    }

    private static (string token, long chatId) GetTokenAndChat(WebApplicationBuilder builder)
    {
        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(builder.Environment.ContentRootPath)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true);

        var configurationRoot = configurationBuilder.Build();
        var token = Environment.GetEnvironmentVariable("BOT_TOKEN");
        var baseChatId = configurationRoot[$"{nameof(BotConfiguration)}:{nameof(BotConfiguration.BaseChatId)}"];
        TryParse(baseChatId, out var chatId);
        return (token, chatId);
    }
}