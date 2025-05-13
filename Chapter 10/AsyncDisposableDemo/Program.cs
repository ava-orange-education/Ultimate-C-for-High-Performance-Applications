
await using var resource = new AsyncResource();
Console.WriteLine("Using async resource...");
Console.WriteLine(await resource.GetDataAsync());


public class AsyncResource : IAsyncDisposable
{
    private bool _disposed;

    public async ValueTask DisposeAsync()
    {
        if (!this._disposed)
        {
            await Task.Delay(100);
            Console.WriteLine("Async resources cleaned up.");
            this._disposed = true;
        }
    }

    public async Task<string> GetDataAsync()
    {
        await Task.Delay(100);
        return "Retrieved data.";
    }
}