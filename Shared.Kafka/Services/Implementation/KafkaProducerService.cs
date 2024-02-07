using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Shared.Kafka.Extensions;
using Shared.Kafka.Options;

namespace Shared.Kafka.Services.Implementation;

internal class KafkaProducerService<T> : IKafkaProducerService<T>
{
    private readonly ILogger _logger;
    private readonly string _topicName;
    private readonly IProducer<string, string>? _producer;

    public KafkaProducerService(
        ILogger<KafkaProducerService<T>> logger,
        IOptions<KafkaProducerConfiguration> kafkaOptions, 
        KafkaTopicSource topicSource)
    {
        _logger = logger;

        _topicName = kafkaOptions.Value.Connections.TryGetValue(topicSource, out var topicName) 
            ? topicName 
            : throw new ArgumentNullException($"Topic name was not found for {nameof(KafkaTopicSource)} {topicSource}");
            
        try
        {
            var producerConfiguration = kafkaOptions.Value.Producers.TryGetValue(topicSource, out var producerConfigurationValue) 
                ? producerConfigurationValue
                : new ProducerConfig();

            _producer = ProducerBuilder(kafkaOptions.Value, producerConfiguration).Build();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error occurred while trying to build {KafkaService}. Service will not start", nameof(KafkaProducerService<T>));
        }
    }
        
    public async Task<DeliveryResult<string, string>> ProduceAsync(
        T value, 
        IDictionary<string, string>? headers = null, 
        CancellationToken cancellationToken = default)
    {
        if (_producer == null)
        {
            throw new InvalidOperationException("Invalid operation. Kafka producer is null");
        }
        
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value), "Value of Kafka message can not be null");
        }
        
        try
        {
            var message = BuildMessage(value, headers);
            var deliveryResult = await _producer.ProduceAsync(_topicName, message, cancellationToken);
            
            return deliveryResult;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Can't publish {Name} model into Kafka\n" + 
                                        "Exception message: {Message}\n" + 
                                        "Inner exception message: {InnerMessage}", 
                typeof(T).Name, exception.Message, exception.InnerException?.Message);
                
            throw;
        }
    }

    public void Produce(
        T value, 
        IDictionary<string, string>? headers = null, 
        Action<DeliveryReport<string, string>>? deliveryHandler = null)
    {
        if (_producer == null)
        {
            throw new InvalidOperationException("Invalid operation. Kafka producer is null");
        }
        
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value), "Value of Kafka message can not be null");
        }
        
        try
        {
            var message = BuildMessage(value, headers);
            _producer.Produce(_topicName, message, deliveryHandler);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Can't publish {Name} model into Kafka\n" + 
                                        "Exception message: {Message}\n" + 
                                        "Inner exception message: {InnerMessage}", 
                typeof(T).Name, exception.Message, exception.InnerException?.Message);
                
            throw;
        }
    }
        
    private static Message<string, string> BuildMessage(T value, IDictionary<string, string>? headers)
    {
        var messageHeaders = BuildHeaders(headers);

        var message = new Message<string, string>
        {
            Key = headers == null ? string.Empty : JsonConvert.SerializeObject(headers),
            Value = value == null ? string.Empty : JsonConvert.SerializeObject(value),
            Headers = messageHeaders
        };

        return message;
    }

    private static Headers BuildHeaders(IDictionary<string, string>? headers)
    {
        var kafkaHeaders = new Headers();

        if (headers != null && headers.Any())
        {
            foreach (var (key, value) in headers)
            {
                kafkaHeaders.AddHeader(key, value);
            }
        }

        return kafkaHeaders;
    }

    private ProducerBuilder<string, string> ProducerBuilder(KafkaConfigurationBase baseConfiguration, ProducerConfig producerConfiguration)
    {
        producerConfiguration.BootstrapServers = baseConfiguration.Brokers;
        producerConfiguration.SecurityProtocol = SecurityProtocol.Plaintext;
        var producerBuilder = new ProducerBuilder<string, string>(producerConfiguration)
            .SetErrorHandler((_, error) =>
            {
                _logger.LogError("Error occurred in Kafka producer for {Topic} topic. " +
                                 "Reason={Reason}, Code={Code}, " +
                                 "IsBrokerError={IsBrokerError}, IsLocalError={IsLocalError}",
                    _topicName, error.Reason, error.Code, error.IsBrokerError, error.IsLocalError);
            });

        return producerBuilder;
    }
        
    public void Dispose()
    {
        _producer?.Dispose();
    }
}