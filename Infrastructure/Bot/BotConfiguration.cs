namespace Infrastructure.Bot;

public class BotConfiguration
{
    //public string BotToken { get; set; }
    public string Socks5Host { get; set; }
    public int Socks5Port { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Version { get; set; }
    public string ConfigType { get; set; }
    public long DefaultChatId { get; set; }
    public bool IsPoxyEnabled { get; set; } = false;
    public long BaseChatId { get; set; }
}