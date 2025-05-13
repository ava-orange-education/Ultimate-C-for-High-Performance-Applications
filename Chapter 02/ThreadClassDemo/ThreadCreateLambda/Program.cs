namespace ThreadClassDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var workerThread = new Thread(() => Worker.PrintMessage());
        }
    }

    internal class Worker
    {
        public static void PrintMessage()
        {
            Console.WriteLine("Hello world from another thread!");
        }
    }
}
