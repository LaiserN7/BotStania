using Domain.Extensions;

namespace Domain.Constants;

public static class InlineCommands
{
    public const string Info = "/info";
    public const string ChatId = "/chatId";
    public const string Version = "/version";
    public const string Hello = "/hello";
    public const string SetTrustedUser = "/setTrustedUser";
    
    public static string[] AllKeys => typeof(InlineCommands).GetAllKeys();
}