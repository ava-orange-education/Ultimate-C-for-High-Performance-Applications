
var cts = new CancellationTokenSource();
var token = cts.Token;

token.Register(() =>
{
    Console.WriteLine("Cancellation has been requested. Performing cleanup...");
});

cts.Cancel();
