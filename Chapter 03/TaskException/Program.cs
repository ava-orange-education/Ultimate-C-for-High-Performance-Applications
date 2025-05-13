var task = Task.Run(() => ProcessData(0));

try
{
    task.Wait();
}
catch (AggregateException ex)
{
    foreach (var exception in ex.Flatten().InnerExceptions)
        Console.WriteLine(exception.Message);
}

decimal ProcessData(int number)
{
    if (number == 0)
    {
        throw new ArgumentException("Invalid argument.");
    }

    return 100.0M / number;
}