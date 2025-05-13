using System.Collections.Concurrent;

var tasks = new ConcurrentBag<Task>();

tasks.Add(Task.Run(() =>
{
    tasks.Add(Task.Run(() =>
    {
        throw new InvalidOperationException("Child task exception.");
    }));

    Thread.Sleep(100);
    Console.WriteLine("Main task completed.");
}));

Thread.Sleep(100); //ensure all tasks are scheduled.
try
{
    Task.WaitAll(tasks.ToList());
}
catch (AggregateException ex)
{
    Console.WriteLine(ex.Flatten().Message);
}

Console.WriteLine("Work completed.");