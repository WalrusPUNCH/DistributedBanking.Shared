using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Shared.Redis.Models;
using Shared.Redis.Options;
using StackExchange.Redis;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks.Dataflow;

namespace Shared.Redis.Services.Implementation;

public class RedisProvider : IRedisProvider, IRedisSubscriber
{
    private readonly ConfigurationOptions _configurationOptions;
    private Lazy<ConnectionMultiplexer> Connection => new (() => ConnectionMultiplexer.Connect(_configurationOptions));
    private IDatabase Database => Connection.Value.GetDatabase();
    private ISubscriber Subscriber => Connection.Value.GetSubscriber();
    
    private readonly JsonSerializerSettings _defaultJsonSettings;
    private readonly ILogger<RedisProvider> _logger;

    public RedisProvider(
        ILogger<RedisProvider> logger,
        IOptions<RedisOptions> options,
        Action<ConfigurationOptions>? configureConnection = null)
    {
        var configurationOptions = ConfigurationOptions.Parse(options.Value.ConnectionString, true);
        configureConnection?.Invoke(configurationOptions);
        _configurationOptions = configurationOptions;
        
        _defaultJsonSettings = new JsonSerializerSettings 
        { 
            NullValueHandling = NullValueHandling.Ignore,
            Converters = new List<JsonConverter> 
            {
                new StringEnumConverter()
            }
        };
        
        _logger = logger;
            
        Database.Multiplexer.ConnectionFailed += (_, ev) =>
        {
            _logger.LogError(ev.Exception, "Redis connection ({ConnectionType}) to the ({@Endpoint}) has failed! " + 
                                           "Failure: {FailureType}", ev.ConnectionType, ev.EndPoint, ev.FailureType);
        };

        Database.Multiplexer.ConnectionRestored += (_, ev) =>
        {
            _logger.LogInformation("Redis connection ({ConnectionType}) to the ({@Endpoint}) has restored!", 
                ev.ConnectionType, ev.EndPoint);
        };
    }

    #region Redis provider
    
    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await Database.StringGetAsync(key);
        
        return value.IsNullOrEmpty ? default : JsonConvert.DeserializeObject<T>(value!);
    }
    
    public Task SetAsync<T>(string key, T value, TimeSpan ttl)
        => SetAsync(key, value, ttl, null);

    public async Task SetAsync<T>(string key, T value, TimeSpan ttl, IContractResolver? contractResolver)
    {
        if (contractResolver != null)
        {
            _defaultJsonSettings.ContractResolver = contractResolver;
        }

        var json = JsonConvert.SerializeObject(value, _defaultJsonSettings);
        await Database.StringSetAsync(key, json, ttl);
    }

    public Task SetManyAsync<T>(IReadOnlyDictionary<string, T> values, TimeSpan ttl)
    {
        var batch = Database.CreateBatch();

        var tasks = values.Select(s =>
        {
            var json = JsonConvert.SerializeObject(s.Value);
            return batch.StringSetAsync(s.Key, json, ttl);
        }).ToArray();

        batch.Execute();
        return Task.WhenAll(tasks);
    }
    
    public async Task<bool> DeleteAsync(string key)
    {
        return await Database.KeyDeleteAsync(key);
    }
    
    public Task<long> DeleteManyAsync(IEnumerable<string> keys)
    {
        return Database.KeyDeleteAsync(keys.Select(x => (RedisKey) x).ToArray(), CommandFlags.FireAndForget);
    }
    
    #endregion

    #region Redis subscriber

    public async Task<IAsyncDisposable> SubAsync<T>(string channel, ActionBlock<T> action)
    {
        await Subscriber.SubscribeAsync(channel, Handler, CommandFlags.FireAndForget).ConfigureAwait(false);

        return new AsyncDisposeProxy(() => Subscriber.UnsubscribeAsync(channel));

        void Handler(RedisChannel redisChannel, RedisValue value)
        {
            if (value != default)
            {
                var msg = JsonConvert.DeserializeObject<T>(value!);
                action.Post(msg!);
            }
        }
    }
    
    public async Task PubAsync<T>(string channel, T value)
    {
        var json = JsonConvert.SerializeObject(value, _defaultJsonSettings);
        await Subscriber.PublishAsync(channel, json, CommandFlags.FireAndForget);
    }

    public async Task PubManyAsync<T>(string channel, IReadOnlyDictionary<string, T> values)
    {
        var tasks = values.Select(s =>
        {
            var json = JsonConvert.SerializeObject(s.Value);
            var entityChannelName = s.Key;
            return Subscriber.PublishAsync(entityChannelName, json, CommandFlags.FireAndForget);
        }).ToArray();

        await Task.WhenAll(tasks);
    }
    
    public IObservable<T> ObserveChannel<T>(string channel)
    {
        return Observable.Create<T>(observer =>
        {
            // Subscribe to the Redis channel
            Subscriber.Subscribe(channel, Handler, CommandFlags.FireAndForget);

            // Return a disposal method to unsubscribe from the channel when the Observable is disposed
            return Disposable.Create(() => Subscriber.Unsubscribe(channel, Handler, CommandFlags.FireAndForget));

            // The handler which will be executed on every received Redis message
            void Handler(RedisChannel redisChannel, RedisValue value)
            {
                try
                {
                    if (value != default)
                    {
                        var message = JsonConvert.DeserializeObject<T>(value!);
                        observer.OnNext(message!); 
                        observer.OnCompleted();
                    }
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Error occurred while trying to observe '{Channel}' channel", channel);
                    observer.OnError(exception);
                }
            }
        });
    }
    
    public async Task<T> SingleObserveChannel<T>(string channel)
    {
        return await Observable.Create<T>(observer =>
        {
            // Subscribe to the Redis channel
            Subscriber.Subscribe(channel, Handler, CommandFlags.FireAndForget);

            // Return a disposal method to unsubscribe from the channel when the Observable is disposed
            return Task.FromResult(Disposable.Create(() => Subscriber.Unsubscribe(channel, Handler, CommandFlags.FireAndForget)));

            // The handler which will be executed on every received Redis message
            void Handler(RedisChannel redisChannel, RedisValue value)
            {
                try
                {
                    if (value != default)
                    {
                        var message = JsonConvert.DeserializeObject<T>(value!);
                        observer.OnNext(message!);
                        observer.OnCompleted();
                    }
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Error occurred while trying to observe '{Channel}' channel", channel);
                    observer.OnError(exception);
                }
            }
        }).SingleAsync();
    }
    
    public async Task UnsubAsync(string channel)
    {
        await Subscriber.UnsubscribeAsync(
            channel,
            null, CommandFlags.FireAndForget).ConfigureAwait(false);
    }
    
    public void Unsub(string channel)
    {
        Subscriber.Unsubscribe(channel, null, CommandFlags.FireAndForget);
    }

    #endregion
}