using System.Collections.Concurrent;

var queue = new BlockingCollection<string>();
try
{
    var producer = Task.Run(PlaceOrders);
    var consumer = Task.Run(FulfillOrders);

    Task.WaitAll(producer, consumer);

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
    while (!queue.IsCompleted)
    {
        var order = queue.Take();
        Thread.Sleep(200);
        Console.WriteLine("Fulfilled " + order);
    }
}