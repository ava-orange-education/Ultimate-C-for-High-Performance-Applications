using System.Timers;

Console.WriteLine("Awaiting event-based timer...");
await WaitForTimerAsync(1000);
Console.WriteLine("Timer expired.");

var cts = new CancellationTokenSource(250);
try
{
    Console.WriteLine("Cancelling...");
    await WaitForTimerAsync(1000, cts.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Operation cancelled.");
}

Task WaitForTimerAsync(int milliseconds, CancellationToken token = default)
{
    var tcs = new TaskCompletionSource<bool>();
    var timer = new System.Timers.Timer(milliseconds);

    ElapsedEventHandler? handler = null;
    handler = (sender, e) =>
    {
        timer.Elapsed -= handler!;
        timer.Dispose();
        tcs.TrySetResult(true); // Complete the task
    };

    timer.Elapsed += handler;
    timer.AutoReset = false;
    timer.Start();

    if (token.CanBeCanceled)
    {
        token.Register(() =>
        {
            timer.Elapsed -= handler!;
            timer.Dispose();
            tcs.TrySetCanceled(); // Cancel the task
        });
    }

    return tcs.Task;
}
