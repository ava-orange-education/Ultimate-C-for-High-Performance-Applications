var cts = new CancellationTokenSource();

var backgroundThread = new Thread(() =>
{
    while (!cts.Token.IsCancellationRequested)
    {
        Thread.Sleep(1000);
        Console.WriteLine("Handling low priority processing.");
    }
})
{
    Priority = ThreadPriority.BelowNormal,
    IsBackground = true
};
backgroundThread.Start();

Console.WriteLine("Press any key to exit.");
Console.ReadKey();
cts.Cancel();

backgroundThread.Join();
Console.WriteLine("Background thread stopped.");