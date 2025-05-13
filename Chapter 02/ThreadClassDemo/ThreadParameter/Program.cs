namespace ThreadClassDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var workerThread = new Thread(() => Worker.PrintMessage(5));
            workerThread.Start();
        }
    }

    internal class Worker
    {
        public static void PrintMessage(int number)
        {
            Console.WriteLine($"You passed the number: {number}");
        }
    }
}
