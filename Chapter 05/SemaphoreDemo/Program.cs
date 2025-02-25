var pool = new Queue<string>(["One", "Two", "Three"]);
var lockObject = new Lock();
var semaphore = new SemaphoreSlim(3, 3);
try
{
    var threadList = new List<Thread>();
    for (int i = 0; i < 10; i++)
    {
        threadList.Add(new Thread(UseResource));
        threadList[i].Start();
    }

    foreach (var thread in threadList)
        thread.Join();

    Console.WriteLine("Work completed.");
}
finally
{
    semaphore.Dispose();
}

void UseResource()
{
    semaphore.Wait();
    try
    {
        var resource = Dequeue();
        Console.WriteLine($"Using resource {resource}");
        Thread.Sleep(1000);
        Enqueue(resource);
    }
    finally
    {
        semaphore.Release();
    }
}

string Dequeue()
{
    lock (lockObject)
    {
        return pool.Dequeue();
    }
}

void Enqueue(string resource)
{
    lock (lockObject)
    {
        pool.Enqueue(resource);
    }
}