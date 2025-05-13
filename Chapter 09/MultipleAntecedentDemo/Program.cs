Console.WriteLine("Starting tasks...");
var task1 = Task.Run(() =>
{
    Thread.Sleep(1000);
    Console.WriteLine("Completed task 1.");
});

var task2 = Task.Run(() =>
{
    Thread.Sleep(1000);
    Console.WriteLine("Completed task 2.");
});

Task continuation = Task.Factory.ContinueWhenAll([task1, task2], antecedents =>
{
    Console.WriteLine("Completed continuation.");
});

continuation.Wait();
Console.WriteLine("All tasks completed.");