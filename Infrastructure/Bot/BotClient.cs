using System.Net;
using System.Security.Authentication;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace Infrastructure.Bot;

public class BotClient
{
    private ITelegramBotClient Client { get; set; }
    private BotConfiguration BotConfig { get; }

    public BotClient(IOptions<BotConfiguration> config) =>
        (BotConfig, Client) = (config.Value, null);
    
    public ITelegramBotClient GetBotClient()
    {
        if (Client is not null) return Client;
        Client = BuildTelegramBotClient();
        return Client;
    }
    private TelegramBotClient BuildTelegramBotClient()
    {
        return BotConfig.IsPoxyEnabled
            ? new TelegramBotClient(
                Environment.GetEnvironmentVariable("BOT_TOKEN"),
                GetProxyClient(BotConfig.Host, BotConfig.Port, BotConfig.UserName, BotConfig.Password))
            : new TelegramBotClient(Environment.GetEnvironmentVariable("BOT_TOKEN"));
    }

    private static HttpClient GetProxyClient(string host, int port, string username, string password)
    {
        var proxy = new WebProxy($"{host}:{port}/", false, Array.Empty<string>(),
            new NetworkCredential(username, password));

        var httpClientHandler = new HttpClientHandler
        {
            Proxy = proxy,
            SslProtocols = SslProtocols.Tls
        };

        return new HttpClient(httpClientHandler, true);
    }
}