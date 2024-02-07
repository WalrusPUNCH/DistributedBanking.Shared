using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Redis.Services;
using Shared.Redis.Services.Implementation;

namespace Shared.Redis.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddRedisCache(this IServiceCollection services, string connectionString)
    {
         services.AddSingleton<IRedisProvider>(sp =>
            new RedisProvider(sp.GetRequiredService<ILogger<RedisProvider>>(), connectionString));
         
         services.AddSingleton<IRedisSubscriber>(sp =>
             new RedisProvider(sp.GetRequiredService<ILogger<RedisProvider>>(), connectionString));
         
         return services;
    }
}