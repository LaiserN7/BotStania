using API.Helpers;

namespace API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private ILogger<ExceptionHandlingMiddleware> Logger { get; }
    private RequestDelegate Next { get; }
    private const string DefaultMessage = "Something went wrong";


    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        Logger = logger;
        Next = next;

    }

    public async Task InvokeAsync(HttpContext context)
    {
        //var body = await Request.GetBody(context.Request);
        var (chatId, message) = await Request.GetInfo(context.Request);
        try
        {
            await Next.Invoke(context);
        }
        catch (Exception exception)
        {
            //Logger.LogInformation(body); 
            OnException(exception, chatId, message);
        }
    }

    private void OnException(Exception exception, long chatId, string message)
    {
        Logger.LogCritical( exception, DefaultMessage);
        Logger.LogError(exception.Message);
    }
}