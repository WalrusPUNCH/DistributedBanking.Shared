namespace Shared.Redis.Models;

public class AsyncDisposeProxy : IAsyncDisposable
{
    private readonly Func<Task> _onDispose;

    public AsyncDisposeProxy(Func<Task> onDispose)
    {
        _onDispose = onDispose;
    }

    public async ValueTask DisposeAsync()
    {
        await _onDispose();   
    }
}