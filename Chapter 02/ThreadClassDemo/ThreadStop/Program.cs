namespace ThreadClassDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var counter = new Counter();
            var workerThread = new Thread(() => counter.CountTo(10));
            workerThread.Start();

            Console.WriteLine("Main thread waiting for counting to complete.");
            Thread.Sleep(1000);
            counter.StopRunning = true;

            workerThread.Join();

            Console.WriteLine("Main thread completed.");
        }
    }

    internal class Counter
    {
        public bool StopRunning { get; set; }

        public void CountTo(int number)
        {
            for (int i = 1; i < number + 1; i++)
            {
                if (StopRunning)
                {
                    Console.WriteLine("Counter stopping.");
                    break;
                }
                Console.WriteLine($"Count: {i}");
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }
    }
}
