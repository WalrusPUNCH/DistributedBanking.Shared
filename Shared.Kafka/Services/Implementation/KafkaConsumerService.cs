using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Shared.Kafka.Messages;
using Shared.Kafka.Options;
using System.Reactive.Linq;

namespace Shared.Kafka.Services.Implementation;

internal class KafkaConsumerService<TKey, TValue> : IKafkaConsumerService<TKey, TValue>
{
    private readonly ILogger<KafkaConsumerService<TKey, TValue>> _logger;
    private readonly string _topicName;

    private readonly IConsumer<string, string>? _consumer;
        
    public KafkaConsumerService(
        ILogger<KafkaConsumerService<TKey, TValue>> logger,
        IOptions<KafkaConsumerConfiguration> kafkaConsumerOptions,
        KafkaTopicSource topicSource)
    {
        _logger = logger;
        
        _topicName = kafkaConsumerOptions.Value.Connections.TryGetValue(topicSource, out var topicName) 
            ? topicName 
            : throw new ArgumentOutOfRangeException($"Topic name was not found for {nameof(KafkaTopicSource)} {topicSource}");
       
        try
        {
            var consumerConfiguration = kafkaConsumerOptions.Value.Consumers.TryGetValue(topicSource, out var consumerConfigurationValue) 
                ? consumerConfigurationValue 
                : kafkaConsumerOptions.Value.Consumers.TryGetValue(KafkaTopicSource.Default, out var defaultConsumerConfigurationValue) 
                    ? defaultConsumerConfigurationValue 
                    : throw new ArgumentOutOfRangeException(
                        $"Consumer configuration for {nameof(KafkaTopicSource)} {topicSource} was not found " +
                        "and default configuration is missing too");
            
            _consumer = GetConsumerBuilder(consumerConfiguration).Build();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error occurred while trying to build {KafkaService} for {Topic}. Service will not start", nameof(KafkaConsumerService<TKey, TValue>), _topicName);
        }
    }
        
    public IObservable<MessageWrapper<TValue>> Consume(CancellationToken cancellationToken = default)
    {
        return ConsumeInternal(default, cancellationToken);
    }

    public IObservable<MessageWrapper<TValue>> Consume(TimeSpan timeout)
    {
        return ConsumeInternal(timeout, default);
    }
        
    private IObservable<MessageWrapper<TValue>> ConsumeInternal(TimeSpan timeout, CancellationToken cancellationToken)
    {
        if (_consumer == null)
        {
            return Observable.Empty<MessageWrapper<TValue>>();
        }
        
        var consumeTimeout = timeout != default 
            ? timeout 
            : TimeSpan.FromSeconds(10);
            
        _consumer.Subscribe(_topicName);

        return Observable.Create<MessageWrapper<TValue>>(async observer =>
        {
            var traceLog = new List<string>();
            while (!cancellationToken.IsCancellationRequested)
            {
                traceLog.Clear();
                try
                {
                    var consumeResult = _consumer.Consume(consumeTimeout);
                    if (consumeResult is null)
                    {
                        await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
                        continue;
                    }

                    traceLog.Add($"[{_topicName}] trying to deserialize message value: '{consumeResult.Message?.Value}'");

                    if (consumeResult.Message?.Value == null)
                    {
                        _logger.LogWarning("Null message value has been consumed from topic {Topic}", _topicName);
                        continue;
                    }

                    var offset = consumeResult.TopicPartitionOffset;
                    var consumeValue = JsonConvert.DeserializeObject<TValue>(consumeResult.Message.Value);
                    observer.OnNext(new MessageWrapper<TValue>(offset, consumeValue!));
                }
                catch (OperationCanceledException e)
                {
                    _logger.LogWarning(e, "Closing consumer for topic {Topic}", _topicName);
                    observer.OnCompleted();
                    
                    break;
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Unhandled error occured while consuming message from '{Topic}' topic. " +
                                        "Log: {TraceLog}", _topicName, traceLog); //string.Join(" ", traceLog);
                }
            }
                
            return () => _logger.LogInformation("Disposing consumer for {TopicName}", _topicName);
        }).Retry();
    }

    private ConsumerBuilder<string, string> GetConsumerBuilder(ConsumerConfig config)
    {
        var consumerBuilder = new ConsumerBuilder<string, string>(config);

        return consumerBuilder
            .SetErrorHandler((_, error) =>
            {
                _logger.LogError(
                    "Error occured in Kafka consumer for {Topic} topic. Reason={Reason}, Code={Code}, " +
                    "IsBrokerError={IsBrokerError}, IsLocalError={IsLocalError}",
                    _topicName, error.Reason, error.Code, error.IsBrokerError, error.IsLocalError);
            })
            .SetStatisticsHandler((_, json) => _logger.LogDebug("Statistics: {Stat}", json));
    }
   
    public void Dispose()
    {
        _consumer?.Dispose();
    }
}