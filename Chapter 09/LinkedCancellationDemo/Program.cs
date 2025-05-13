using var userCts = new CancellationTokenSource();
using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
using var linkedCts =
    CancellationTokenSource.CreateLinkedTokenSource(userCts.Token, timeoutCts.Token);

CancellationToken linkedToken = linkedCts.Token;

var task = Task.Run(() =>
{
    for (var i = 0; i < 11; i++)
    {
        linkedToken.ThrowIfCancellationRequested();
        Console.WriteLine($"Processing iteration {i}");
        Thread.Sleep(1000);
    }
}, linkedToken);

var userTask = Task.Run(() =>
{
    Console.ReadLine();
    userCts.Cancel();
});

try
{
    task.Wait();
    Console.WriteLine("Operation completed successfully.");
}
catch (AggregateException ex)
when (ex.InnerExceptions.First() is OperationCanceledException)
{
    Console.WriteLine("Operation cancelled.");
}


