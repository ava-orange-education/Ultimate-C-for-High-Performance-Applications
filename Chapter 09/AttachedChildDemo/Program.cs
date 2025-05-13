Task mainTask = Task.Factory.StartNew(() =>
{
    Task childTask = Task.Factory.StartNew(() =>
    {
        Thread.Sleep(1000);
        Console.WriteLine("Child task completed.");
    }, TaskCreationOptions.AttachedToParent);

    Thread.Sleep(500);
    Console.WriteLine("Main task completed.");
});

mainTask.Wait();
Console.WriteLine("Work completed.");
