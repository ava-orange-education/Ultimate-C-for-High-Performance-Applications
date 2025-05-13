var mainThreadId = Thread.CurrentThread.ManagedThreadId;

ThreadPool.QueueUserWorkItem((string state) =>
{
    Console.WriteLine($"{state} using Action<TState>.");
}, "Hello there!", false);

await Task.Delay(100);
Console.WriteLine($"Completed main thread: {mainThreadId}.");
