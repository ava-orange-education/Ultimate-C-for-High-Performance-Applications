using System.Collections.Concurrent;

var cts = new CancellationTokenSource();
var buffer = new BlockingCollection<string>();
try
{
    var producer = Task.Run(() => BufferWork(cts.Token));
    var consumer = Task.Run(() => ProcessWork(cts.Token));

    Thread.Sleep(5000);
    cts.Cancel();
    Task.WaitAll(producer, consumer);

    Console.WriteLine("Items remaining in the buffer: " + buffer.Count);
}
finally
{
    buffer.Dispose();
}

void BufferWork(CancellationToken token)
{
    int count = 1;
    while (!token.IsCancellationRequested)
    {
        if (buffer.TryAdd($"WorkItem {count++}", Timeout.Infinite, token))
            Thread.Sleep(100);
    }
}

void ProcessWork(CancellationToken token)
{
    try
    {
        foreach (var workItem in buffer.GetConsumingEnumerable(token))
        {
            Console.WriteLine("Processing work item " + workItem);
            Thread.Sleep(200);
        }
    }
    catch (OperationCanceledException) { }
}