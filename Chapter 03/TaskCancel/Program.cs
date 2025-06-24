
using System.Threading;
using System.Threading.Tasks;

var cts = new CancellationTokenSource();
var token = cts.Token;

var task = Task.Run(() =>
{
    while (!token.IsCancellationRequested)
    {
        Console.WriteLine("Task working...");
        Thread.Sleep(1000);
    }

    Console.WriteLine("Task cancelled.");

}, token);

Thread.Sleep(4500);
cts.Cancel();
task.Wait();
cts.Dispose();
Console.Read();
