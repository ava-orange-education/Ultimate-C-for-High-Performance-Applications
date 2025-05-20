using System.Diagnostics;

namespace ThreadClassDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var workerThread = new Thread(() => Counter.CountTo(10));
            workerThread.Start();

            Console.WriteLine("Main thread completed.");
        }
    }

    internal class Counter
    {
        public static void CountTo(int number)
        {
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 1; i < number + 1; i++)
            {
                Console.WriteLine($"Count: {i} (Elapsed seconds: {sw.Elapsed.Seconds})");
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }
    }
}
