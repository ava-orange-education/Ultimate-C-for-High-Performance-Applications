var cts = new CancellationTokenSource();
var backgroundTask = Task.Run(async () =>
{
    while (!cts.Token.IsCancellationRequested)
    {
        await Task.Delay(1000, cts.Token);
        Console.WriteLine("Processed work iteration...");
    }
});

Console.WriteLine("Press a key to exit.");
Console.ReadKey();

cts.Cancel();
try
{
    await backgroundTask;
}
catch (OperationCanceledException)
{
}

Console.WriteLine("Background task exited.");