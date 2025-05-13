namespace DataSyncDemo;

internal class Program
{
    static List<string> sharedList = new();
    static Lock lockObject = new();

    static void Main(string[] args)
    {
        var thread1 = new Thread(() => AddItems());
        var thread2 = new Thread(() => AddItems());
        thread1.Name = "Thread One";
        thread2.Name = "Thread Two";

        thread1.Start();
        thread2.Start();

        thread1.Join();
        thread2.Join();

        foreach (var message in sharedList)
            Console.WriteLine(message);
    }

    static void AddItems()
    {
        for (int i = 0; i < 10; i++)
        {
            lock (lockObject)
            {
                if (i % 2 == 0)
                    sharedList.Add($"{Thread.CurrentThread.Name} - {i}");
            }
        }
    }

}
