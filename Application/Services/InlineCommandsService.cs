using System.Text;
using Application.Abstractions;
using Domain.Constants;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class InlineCommandsService : IInlineCommandService
{
    private IBotService BotService { get; }
    private ILogger<InlineCommandsService> Logger { get; }
    private ITrustedUsersService TrustedUsersService { get; }
    private long UserId { get; set; }
    private long ChatId { get; set; }


    public InlineCommandsService(IBotService botService, ILogger<InlineCommandsService> logger,
        ITrustedUsersService trustedUsersService)
    {
        BotService = botService;
        Logger = logger;
        TrustedUsersService = trustedUsersService;
    }

    public Task<bool> IsCommand(string command) => Task.FromResult(InlineCommands.AllKeys.Contains(command));

    public async Task HandleCommand(string command, long userId, long chatId)
    {
        UserId = userId;
        ChatId = chatId;
        if (!TrustedUsersService.IsEmpty() && !TrustedUsersService.IsExist(userId))
            await HandleNoAccessResponse();

        await HandleCommand(command);
    }

    private async Task HandleNoAccessResponse() =>
        await BotService.SendTextMessageAsync(ChatId, ResponseText.AccessDenied);

    private async Task HandleCommand(string command)
    {
        switch (command.Split(' ').First())
        {
            case InlineCommands.Hello:
                await BotService.SendTextMessageAsync(ChatId, ResponseText.Hello);
                break;
            case InlineCommands.ChatId:
                await BotService.SendTextMessageAsync(ChatId, ChatId.ToString());
                break;
            case InlineCommands.Info:
                await BotService.SendTextMessageAsync(ChatId, BuildInfo());
                break;
            case InlineCommands.SetTrustedUser:
                var added =await TryAddUser();
                var message = added ? ResponseText.Success : ResponseText.Failed;
                await BotService.SendTextMessageAsync(ChatId, message);
                break;
            default: throw new NotSupportedException("Unknown command");
        }
    }

    private Task<bool> TryAddUser()
    {
        var added = false;
        try
        {
            added = TrustedUsersService.Add(new TrustedUser(UserId));
        }
        catch (Exception e)
        {
            Logger.LogError(e, "User not added");
        }

        return Task.FromResult(added);
    } 

    private static string BuildInfo()
    {
        var sb = new StringBuilder();
        sb.AppendLine("Hello from STANIA BOT");
        sb.AppendLine("______________");
        sb.AppendLine("Usages commands");
        foreach (var key in InlineCommands.AllKeys)
        {
            sb.AppendLine(key);
        }

        return sb.ToString();
    }
}