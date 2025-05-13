using System.Collections.Concurrent;

ConcurrentBag<string> pool = new ConcurrentBag<string>()
{
    "Resource 1",
    "Resource 2"
};
var threadList = new List<Thread>();
for (int i = 0; i < 4; i++)
{
    var thread = new Thread(UseResource);
    thread.Start();
    threadList.Add(thread);
}

foreach (var thread in threadList)
    thread.Join();

void UseResource()
{
    string resource;
    if (!pool.TryTake(out resource))
        resource = $"Resource created by thread: {Thread.CurrentThread.ManagedThreadId}";
    Thread.Sleep(500);
    pool.Add(resource);
    Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} used resource: {resource}");
}