using Nito.AsyncEx.Synchronous;
using Serilog.Core;
using Serilog.Events;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Infrastructure.Logger;

public class TelegramSink : ILogEventSink
{
    private IFormatProvider FormatProvider { get; }
    private LogEventLevel MinimumLevel { get; }
    private ITelegramBotClient TelegramBot { get; }
    private long ChatId { get; }

    public TelegramSink(IFormatProvider formatProvider, string telegramApiKey, long chatId,
        LogEventLevel minimumLevel) => (FormatProvider,
            MinimumLevel, ChatId, TelegramBot) =
        (formatProvider, minimumLevel, chatId, new TelegramBotClient(telegramApiKey));

    public void Emit(LogEvent logEvent)
    {
        if (logEvent.Level < MinimumLevel) return;
        var loggedMessage = logEvent.RenderMessage(FormatProvider);
        var task = Task.Run(async () => await TelegramBot.SendTextMessageAsync(new ChatId(ChatId), loggedMessage));
        task.WaitAndUnwrapException();
    }
}