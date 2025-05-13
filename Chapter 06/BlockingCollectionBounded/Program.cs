using System.Collections.Concurrent;

var bounded = new BlockingCollection<string>(2);
var unbounded = new BlockingCollection<string>();

Console.WriteLine("Unbounded work...");
var producer = Task.Run(() => Produce(unbounded));
var consumer = Task.Run(() => Consume(unbounded));
Task.WaitAll(producer, consumer);

Console.WriteLine("Bounded work...");
producer = Task.Run(() => Produce(bounded));
consumer = Task.Run(() => Consume(bounded));
Task.WaitAll(producer, consumer);

Console.WriteLine("Completed.");

void Produce(BlockingCollection<string> collection)
{
    for (int i = 1; i < 6; i++)
    {
        var item = $"WorkItem {i}";
        Console.WriteLine("Producing: " + item);
        collection.Add(item);
    }
    collection.CompleteAdding();
}

void Consume(BlockingCollection<string> collection)
{
    foreach (var item in collection.GetConsumingEnumerable())
    {
        Console.WriteLine("Consumed: " + item);
        Thread.Sleep(200);
    }
}