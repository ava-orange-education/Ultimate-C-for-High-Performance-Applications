
var cts = new CancellationTokenSource();
var token = cts.Token;

var task = Task.Run(() =>
{
    try
    {
        while (true)
        {
            token.ThrowIfCancellationRequested();
            Console.WriteLine("Task working...");
            Thread.Sleep(1000);
        }
    }
    catch (OperationCanceledException)
    {
        Console.WriteLine("Task cancelled.");
        throw;
    }

}, token);

Thread.Sleep(4500);
cts.Cancel();
try { task.Wait(); } catch (AggregateException) { }
Console.WriteLine("The task status is: " + task.Status);
cts.Dispose();
