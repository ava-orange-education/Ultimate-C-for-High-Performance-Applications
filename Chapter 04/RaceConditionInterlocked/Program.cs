namespace DataSyncDemo
{
    internal class Program
    {
        static int loopCount;

        static void Main(string[] args)
        {
            var thread1 = new Thread(() => PlayLoop());
            var thread2 = new Thread(() => PlayLoop());

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            Console.WriteLine($"Final loop count is: {loopCount:N0}");
        }

        static void PlayLoop()
        {
            for (int i = 0; i < 500000; i++)
            {
                Interlocked.Increment(ref loopCount);
            }
        }
    }
}
