using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Kafka.Options;
using Shared.Kafka.Services;
using Shared.Kafka.Services.Implementation;

namespace Shared.Kafka.Extensions;

public static class DependencyInjectionExtensions
{
    private const string KafkaConfigurationSection = "Kafka";

    /// <summary>
    /// Register new Kafka producer
    /// </summary>
    /// <typeparam name="T">Producing type</typeparam>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Configuration</param>
    /// <param name="source">Kafka topic source</param>
    /// <returns>Same service collection</returns>
    public static IServiceCollection AddKafkaProducer<T>(this IServiceCollection services, IConfiguration configuration, KafkaTopicSource source)
    {
        services.Configure<KafkaProducerConfiguration>(configuration.GetSection(KafkaConfigurationSection));

        services.AddSingleton(sp => new KafkaProducerService<T>(
            sp.GetRequiredService<ILogger<KafkaProducerService<T>>>(),
            sp.GetRequiredService<IOptions<KafkaProducerConfiguration>>(),
            source));

        services.AddSingleton<IKafkaProducerService<T>>(x => x.GetRequiredService<KafkaProducerService<T>>());

        return services;
    }
        
    /// <summary>
    /// Register new Kafka consumer
    /// </summary>
    /// <typeparam name="TKey">Kafka message key type</typeparam>
    /// <typeparam name="TValue">Kafka message value type</typeparam>
    /// <param name="services">Service collection</param>
    ///  <param name="configuration">Configuration</param>
    /// <param name="source">Kafka topic source name</param>
    /// <returns>Service collection including consumer</returns>
    public static IServiceCollection AddKafkaConsumer<TKey, TValue>(this IServiceCollection services, IConfiguration configuration, KafkaTopicSource source)
    {
        services.Configure<KafkaConsumerConfiguration>(configuration.GetSection(KafkaConfigurationSection));

        services.AddSingleton(sp => new KafkaConsumerService<TKey, TValue>(
            sp.GetRequiredService<ILogger<KafkaConsumerService<TKey, TValue>>>(),
            sp.GetRequiredService<IOptions<KafkaConsumerConfiguration>>(),
            source));
        
        services.AddSingleton<IKafkaConsumerService<TKey, TValue>>(x => x.GetRequiredService<KafkaConsumerService<TKey, TValue>>());
        
        return services;
    }
}