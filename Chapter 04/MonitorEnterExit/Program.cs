namespace DataSyncDemo;

internal class Program
{
    static object lockObject = new object();
    static int totalCount;
    static int evenCount;

    static void Main(string[] args)
    {
        var thread1 = new Thread(() => PlayLoop());
        var thread2 = new Thread(() => PlayLoop());

        thread1.Start();
        thread2.Start();

        thread1.Join();
        thread2.Join();

        Console.WriteLine($"Total count: {totalCount:N0}");
        Console.WriteLine($"Even count: {evenCount:N0}");
    }

    static void PlayLoop()
    {
        for (int i = 0; i < 500000; i++)
        {
            Monitor.Enter(lockObject);
            try
            {
                totalCount++;
                if (i % 2 == 0)
                {
                    evenCount++;
                }
            }
            finally
            {
                Monitor.Exit(lockObject);
            }
        }
    }
}
