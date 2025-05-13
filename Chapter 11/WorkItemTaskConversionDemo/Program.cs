//Old work item
ThreadPool.QueueUserWorkItem(_ =>
{
    Console.WriteLine("Old work item.");
});

await Task.Delay(100);

//Converted to task
await Task.Run(() =>
{
    Console.WriteLine("Converted to new task.");
});