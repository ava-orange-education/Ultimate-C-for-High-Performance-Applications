namespace ThreadClassDemo
{
    internal class Program
    {
        static bool StopRunning;

        static void Main(string[] args)
        {
            var higherPriorityThread = new Thread(() => Count());
            higherPriorityThread.Priority = ThreadPriority.AboveNormal;
            higherPriorityThread.Name = "higherPriorityThread";

            var normalPriorityThread = new Thread(() => Count());
            normalPriorityThread.Priority = ThreadPriority.Normal;
            normalPriorityThread.Name = "normalPriorityThread";

            var lowerPriorityThread = new Thread(() => Count());
            lowerPriorityThread.Priority = ThreadPriority.BelowNormal;
            lowerPriorityThread.Name = "lowerPriorityThread";

            higherPriorityThread.Start();
            normalPriorityThread.Start();
            lowerPriorityThread.Start();

            Console.WriteLine("Main thread waiting for counting to complete.");
            Thread.Sleep(1000);
            StopRunning = true;

            Console.WriteLine("Main thread completed.");
        }

        static void Count()
        {
            long currentCount = 0;
            while (!StopRunning)
            {
                currentCount++;
            }
            Console.WriteLine($"Thread {Thread.CurrentThread.Name} with priority {Thread.CurrentThread.Priority} counted to: {currentCount:N0}");
        }
    }
}