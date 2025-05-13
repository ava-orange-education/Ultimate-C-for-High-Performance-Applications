var cts = new CancellationTokenSource();
Task backgroundTask = Task.Factory.StartNew(() =>
{
    while (!cts.Token.IsCancellationRequested)
    {
        Thread.Sleep(1000);
        Console.WriteLine("Processed work iteration.");
    }
}, cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);

Console.WriteLine("Press a key to exit.");
Console.ReadKey();

cts.Cancel();
await backgroundTask;

Console.WriteLine("Background task exited.");