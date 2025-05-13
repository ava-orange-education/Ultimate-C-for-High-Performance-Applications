var task1 = Task.Run(() => Counter(10));
var task2 = Task.Run(() => Counter(5));

Task.WaitAll(task1, task2);
Console.WriteLine("Finished all tasks.");

void Counter(int limit)
{
    for (int i = 0; i < limit; i++)
        Thread.Sleep(100);
    Console.WriteLine("Counted to " + limit);
}