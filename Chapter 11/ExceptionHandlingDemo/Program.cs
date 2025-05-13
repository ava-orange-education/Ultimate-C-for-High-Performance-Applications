using System.Collections.Concurrent;

ThreadPool.QueueUserWorkItem((state) =>
{
    throw new InvalidOperationException("This will be unhandled.");
});

ThreadPool.QueueUserWorkItem((state) =>
{
    try
    {
        throw new InvalidOperationException("This will be caught.");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Exception caught: " + ex.ToString());
    }
});

var exceptionQueue = new BlockingCollection<Exception>();

ThreadPool.QueueUserWorkItem((state) =>
{
    try
    {
        throw new InvalidOperationException("Exception for main thread.");
    }
    catch (Exception ex)
    {
        exceptionQueue.Add(ex);
    }
    exceptionQueue.CompleteAdding();
});

Exception exception = exceptionQueue.Take();
Console.WriteLine($"Processed exception: {exception}");

await Task.Delay(100);