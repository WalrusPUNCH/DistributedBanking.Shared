using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Redis.Options;
using Shared.Redis.Services;
using Shared.Redis.Services.Implementation;

namespace Shared.Redis.Extensions;

public static class DependencyInjectionExtensions
{
    private const string RedisConfigurationSection = "Redis";

    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RedisOptions>(configuration.GetSection(RedisConfigurationSection));
        
        services.AddSingleton<IRedisProvider, RedisProvider>();
        services.AddSingleton<IRedisSubscriber, RedisProvider>();
        
        return services;
    }
}