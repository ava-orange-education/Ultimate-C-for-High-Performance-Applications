namespace MonitorTryEnter;

internal class Program
{
    static object lockObject = new object();
    static object secondaryObject = new object();
    static int totalCount;
    static int primaryCount;
    static int secondaryCount;

    static void Main(string[] args)
    {
        var thread1 = new Thread(() => PlayLoop());
        var thread2 = new Thread(() => PlayLoop());

        thread1.Start();
        thread2.Start();

        thread1.Join();
        thread2.Join();

        Console.WriteLine($"Total count: {totalCount:N0}");
        Console.WriteLine($"Primary count: {primaryCount:N0}");
        Console.WriteLine($"Secondary count: {secondaryCount:N0}");
    }

    static void PlayLoop()
    {
        var random = new Random();
        for (int i = 0; i < 100; i++)
        {
            Interlocked.Increment(ref totalCount);
            if (Monitor.TryEnter(lockObject, 50))
            {
                try
                {
                    primaryCount++;
                    var waitPeriod = random.Next(75);
                    Thread.Sleep(waitPeriod);
                }
                finally
                {
                    Monitor.Exit(lockObject);
                }
            }
            else
            {
                Monitor.Enter(secondaryObject);
                try
                {
                    secondaryCount++;
                }
                finally
                {
                    Monitor.Exit(secondaryObject);
                }
            }
        }
    }
}
