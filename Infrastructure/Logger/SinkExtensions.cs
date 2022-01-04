using Serilog;
using Serilog.Configuration;
using Serilog.Events;

namespace Infrastructure.Logger;

public static class SinkExtensions
{
    public static LoggerConfiguration TelegramSink(
        this LoggerSinkConfiguration config, 
        string telegramApiKey, 
        long telegramChatId, 
        IFormatProvider formatProvider = null, 
        LogEventLevel minimumLevel=LogEventLevel.Verbose)
    {
        return config.Sink(new TelegramSink(formatProvider, telegramApiKey, telegramChatId, minimumLevel));
    }
}