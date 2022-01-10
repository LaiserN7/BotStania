using Application.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Application.Services;

public class BotService : IBotService
{
    private ITelegramBotClient BotClient { get; }

    public BotService(ITelegramBotClient telegramBotClient)
    {
        BotClient = telegramBotClient;
    }

    public async Task SendTextMessageAsync(
        long chatId,
        string text,
        CancellationToken cancellationToken = default
    )
    {
        await BotClient.SendTextMessageAsync(new ChatId(chatId), text, cancellationToken: cancellationToken);
    }

    public async Task SendStickerAsync(long chatId, string sticker, CancellationToken cancellationToken = default)
    {
        await BotClient.SendStickerAsync(new ChatId(chatId), sticker, cancellationToken: cancellationToken);
    }

    public async Task SendReplyMessageAsync(long chatId, int replyMessageId, string text,
        CancellationToken cancellationToken = default)
    {
        await BotClient.SendTextMessageAsync(new ChatId(chatId), text, replyToMessageId: replyMessageId,
            cancellationToken: cancellationToken);
    }
}