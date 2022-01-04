using Application.Abstractions;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ITrustedUsersService, TrustedUserService>();
        services.AddScoped<IBotService, BotService>();
        services.AddScoped<IUpdateService, UpdateService>();
        services.AddScoped<IInlineCommandService, InlineCommandsService>();
        return services;
    }
}