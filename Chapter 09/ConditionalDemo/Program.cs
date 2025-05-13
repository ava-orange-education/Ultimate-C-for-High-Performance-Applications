var mainTask = Task.Run(() =>
{
    Thread.Sleep(1000);
    throw new InvalidOperationException("Faulted task.");
});

Task success = mainTask.ContinueWith(antecedent =>
{
    Console.WriteLine("Antecedent succeeded.");
}, TaskContinuationOptions.NotOnFaulted);

Task fail = mainTask.ContinueWith(antecedent =>
{
    Console.WriteLine($"Antecedent faulted with error: {antecedent.Exception.Message}");
}, TaskContinuationOptions.OnlyOnFaulted);

Console.WriteLine("Started tasks");
try
{
    Task.WaitAll(mainTask, success, fail);
}
catch (AggregateException)
{
}
Console.WriteLine("All tasks completed.");