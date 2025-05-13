using System.Collections.Concurrent;

Random rand = new Random();
ConcurrentDictionary<string, int> cache = new ConcurrentDictionary<string, int>();
var threadList = new List<Thread>();
for (int i = 0; i < 10; i++)
{
    var keyName = i % 4;
    var thread = new Thread(() => FetchItemData("item" + keyName));
    thread.Start();
    threadList.Add(thread);
}

foreach (var thread in threadList)
    thread.Join();

Console.WriteLine("Cache contents:");
foreach (var kv in cache)
    Console.WriteLine(kv);

void FetchItemData(string key)
{
    var data = cache.GetOrAdd(key, k =>
    {
        Thread.Sleep(500);
        return rand.Next(1, 100);
    });
    Console.WriteLine($"Retrieved data for key {key}: {data}");
}