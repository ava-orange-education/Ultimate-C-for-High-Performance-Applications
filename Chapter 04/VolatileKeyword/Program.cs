namespace DataSyncDemo
{
    internal class Program
    {
        static volatile bool stopRequested;

        static void Main(string[] args)
        {
            var thread1 = new Thread(() => PlayLoop());
            var thread2 = new Thread(() => PlayLoop());
            thread1.Name = "Thread A";
            thread2.Name = "Thread B";

            thread1.Start();
            thread2.Start();

            Thread.Sleep(2000);
            stopRequested = true;

            thread1.Join();
            thread2.Join();

            Console.WriteLine("Thread work complete.");
        }

        static void PlayLoop()
        {
            int count = 0;
            while (!stopRequested)
            {
                Thread.Sleep(500);
                count++;
                Console.WriteLine($"{Thread.CurrentThread.Name} = {count}");
            }
        }
    }
}