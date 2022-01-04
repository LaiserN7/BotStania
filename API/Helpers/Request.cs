using System.Text;
using Newtonsoft.Json;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace API.Helpers;

public static class Request
{
    private const string DefaultMessagePath = "api/update";

    public static async Task<string> GetBody(HttpRequest request)
        =>
            $"HTTP request information:\n" +
            $"\tMethod: {request.Method}\n" +
            $"\tPath: {request.Path}\n" +
            $"\tQueryString: {request.QueryString}\n" +
            $"\tHeaders: {FormatHeaders(request.Headers)}\n" +
            $"\tSchema: {request.Scheme}\n" +
            $"\tHost: {request.Host}\n" +
            $"\tBody: {await RequestBody(request)}";

    private static async Task<string> RequestBody(HttpRequest request)
    {
        //This line allows us to set the reader for the request back at the beginning of its stream.
        request.EnableBuffering();

        var buffer = new byte[Convert.ToInt32(request.ContentLength)];

        await request.Body.ReadAsync(buffer);

        var bodyAsText = Encoding.UTF8.GetString(buffer);

        request.Body.Position = 0;

        return bodyAsText;
    }

    public static async Task<(long chatId, string message)> GetInfo(HttpRequest request)
    {
        try
        {
            var body = await RequestBody(request);
            var info = $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} \n {body}";

            if (request.Path.Value is not null && !request.Path.Value.ToLower().Contains(DefaultMessagePath))
                return (default, info);

            var update = JsonConvert.DeserializeObject<Update>(body);

            return update is not null && update.Type != UpdateType.Message
                ? (default, info)
#pragma warning disable CS8602
                : (update.Message.Chat.Id, $"{info} \n\n user text: `{update.Message.Text}` \n");
#pragma warning restore CS8602
        }
        catch (Exception exception)
        {
            return (default, exception.Message);
        }
    }

    private static string FormatHeaders(IHeaderDictionary headers) => string.Join(", ",
        headers.Select(kvp => $"{{{kvp.Key}: {string.Join(", ", kvp.Value)}}}"));
}