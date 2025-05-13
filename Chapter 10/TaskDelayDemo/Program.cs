var cts = new CancellationTokenSource(14_000);

for (var i = 1; i < 11; i++)
{
    await Task.Delay(1000);
    Console.WriteLine("Count: " + i);
}

try
{
    while (!cts.IsCancellationRequested)
    {
        await Task.Delay(1000, cts.Token);
        Console.WriteLine("Awaiting cancellation...");
    }
}
catch (OperationCanceledException)
{
    Console.WriteLine("Operation cancelled.");
}