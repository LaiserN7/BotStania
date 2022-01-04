namespace Application.Abstractions;

public interface IBotService
{
    Task SendTextMessageAsync(long chatId, string text, CancellationToken cancellationToken = default);
    Task SendStickerAsync(long chatId, string sticker, CancellationToken cancellationToken = default);
}