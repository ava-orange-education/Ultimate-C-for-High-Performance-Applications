var source = Enumerable.Range(1, 1000000);
var cts = new CancellationTokenSource();
cts.CancelAfter(10);

var thousands = from num in source.AsParallel().WithCancellation(cts.Token)
                where num % 1000 == 0
                select num;

try
{
    foreach (var number in thousands)
        Console.WriteLine(number);
}
catch (OperationCanceledException)
{
    Console.WriteLine("The operation was canceled.");
}