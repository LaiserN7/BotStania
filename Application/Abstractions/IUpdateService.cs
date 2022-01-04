using Telegram.Bot.Types;

namespace Application.Abstractions;

public interface IUpdateService
{
    Task EchoAsync(string json);
}