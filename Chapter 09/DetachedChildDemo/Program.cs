var mainTask = Task.Run(() =>
{
    var childTask = Task.Run(() =>
    {
        Thread.Sleep(1000);
        Console.WriteLine("Child task completed.");
    });
    Thread.Sleep(Random.Shared.Next(500, 1500));
    Console.WriteLine("Main task completed.");
});

mainTask.Wait();
Console.WriteLine("Work completed.");