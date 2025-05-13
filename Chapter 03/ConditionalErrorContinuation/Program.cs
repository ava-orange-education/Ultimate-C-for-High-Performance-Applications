RunContinuation(10);
RunContinuation(0);

void RunContinuation(int number)
{
    var task = Task.Run(() => ProcessData(number));
    task.ContinueWith(antecedent => Console.WriteLine($"The result is: {antecedent.Result}"), TaskContinuationOptions.NotOnFaulted);
    var c2 = task.ContinueWith(antecedent => Console.WriteLine($"Task failed with message: {antecedent.Exception?.Flatten().Message}"), TaskContinuationOptions.OnlyOnFaulted);
    try { c2.Wait(); } catch (AggregateException) { }
}

decimal ProcessData(int number)
{
    if (number == 0)
    {
        throw new ArgumentException("Invalid argument.");
    }

    return 100.0M / number;
}
