using Infrastructure.Bot;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<BotConfiguration>(
            configuration.GetSection(nameof(BotConfiguration)));
        services.AddSingleton<BotClient>();
        services.AddScoped(srv => srv.GetService<BotClient>()?.GetBotClient());
        return services;
    }
}