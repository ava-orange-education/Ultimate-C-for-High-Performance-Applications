var source = new List<string>() { "One", "Two", "Three", "Four", "Five", "Six" };
var cts = new CancellationTokenSource();
var parallelOptions = new ParallelOptions()
{
    CancellationToken = cts.Token
};
cts.CancelAfter(20);

try
{
    Parallel.ForEach(source, parallelOptions, (number, state, index) =>
    {
        Thread.Sleep(50);
        Console.WriteLine(number + " " + index);
    });
}
catch (OperationCanceledException) { }