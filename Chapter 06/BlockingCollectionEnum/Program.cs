using System.Collections.Concurrent;

var taskList = new List<Task>();
var queue = new BlockingCollection<string>();
try
{
    taskList.Add(Task.Run(PlaceOrders));
    taskList.Add(Task.Run(FulfillOrders));
    taskList.Add(Task.Run(FulfillOrders));

    Task.WaitAll(taskList);

    Console.WriteLine("All orders processed.");
}
finally
{
    queue.Dispose();
}

void PlaceOrders()
{
    for (int i = 1; i < 11; i++)
    {
        queue.Add($"Order {i}");
        Thread.Sleep(100);
    }
    queue.CompleteAdding();
}

void FulfillOrders()
{
    foreach (var order in queue.GetConsumingEnumerable())
    {
        Thread.Sleep(200);
        Console.WriteLine("Fulfilled " + order);
    }
}