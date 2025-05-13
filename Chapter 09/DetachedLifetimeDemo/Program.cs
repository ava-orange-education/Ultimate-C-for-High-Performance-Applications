using System.Collections.Concurrent;

var tasks = new ConcurrentBag<Task>();
tasks.Add(Task.Run(() =>
{
    tasks.Add(Task.Run(() =>
    {
        tasks.Add(Task.Run(() =>
        {
            Thread.Sleep(100);
            Console.WriteLine("Child subtask 1 completed.");
        }));

        Thread.Sleep(100);
        Console.WriteLine("Child task 1 completed.");
    }));

    tasks.Add(Task.Run(() =>
    {
        Thread.Sleep(100);
        Console.WriteLine("Child task 2 completed.");
    }));

    Thread.Sleep(100);
    Console.WriteLine("Main task completed.");
}));

Task.WaitAll(tasks.ToList());
Console.WriteLine("All tasks completed.");