namespace API.Middlewares;

public static class CustomMiddlewareExtensions
{
    public static void UseExceptionHandling(this IApplicationBuilder app) => app.UseMiddleware<ExceptionHandlingMiddleware>();
}