namespace Application.Abstractions;

public interface IInlineCommandService
{
    Task<bool> IsCommand(string command);
    Task HandleCommand(string command, long userId, long chatId);
}