Task mainTask = Task.Factory.StartNew(() =>
{
    Task childTask = Task.Factory.StartNew(() =>
    {
        Thread.Sleep(500);
        throw new InvalidOperationException("Child task exception.");
    }, TaskCreationOptions.AttachedToParent);

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

Console.WriteLine("All tasks completed.");