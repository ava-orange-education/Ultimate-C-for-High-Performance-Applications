var mainThreadId = Thread.CurrentThread.ManagedThreadId;
ThreadPool.QueueUserWorkItem(ProcessWork, "Hello there!");
await Task.Delay(100);
Console.WriteLine($"Completed main thread: {mainThreadId}.");

static void ProcessWork(object? state)
{
    var threadId = Thread.CurrentThread.ManagedThreadId;
    Console.WriteLine($"{state} from ThreadPool thread: {threadId}");
}