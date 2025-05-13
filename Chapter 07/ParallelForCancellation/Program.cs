var cts = new CancellationTokenSource();
var parallelOptions = new ParallelOptions
{
    CancellationToken = cts.Token
};
cts.CancelAfter(50);

try
{
    Parallel.For(0, 100, parallelOptions, i =>
    {
        Console.WriteLine("Processing iteration " + i);
        Thread.Sleep(50);
    });
}
catch (OperationCanceledException) { }