var mainTask = Task.Run(() =>
{
    var childTask = Task.Run(() =>
    {
        throw new InvalidOperationException("Child task exception.");
    });

    Thread.Sleep(100);
    Console.WriteLine("Main task completed.");
});

try
{
    mainTask.Wait();
}
catch (AggregateException ex)
{
    Console.WriteLine(ex.Flatten().Message);
}

Console.WriteLine("Work completed.");