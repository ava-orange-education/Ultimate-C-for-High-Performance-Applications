ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();
var threadList = new List<Thread>();
var autoResetEvent = new AutoResetEvent(false);
try
{
    for (int i = 0; i < 2; i++)
    {
        threadList.Add(new Thread(PlayLoop));
        threadList[i].Name = "Thread " + i;
        threadList[i].Start();
    }

    do
    {
        Console.WriteLine("Press a key to do work. Press 'q' to quit.");
        keyInfo = Console.ReadKey();
        autoResetEvent.Set();
    } while (keyInfo.KeyChar != 'q');
    foreach (var thread in threadList)
    {
        autoResetEvent.Set();
        thread.Join();
    }
}
finally
{
    autoResetEvent.Dispose();
}

void PlayLoop()
{
    while (true)
    {
        autoResetEvent.WaitOne();
        if (keyInfo.KeyChar != 'q')
        {
            Console.WriteLine($"{Thread.CurrentThread.Name} working...");
            Thread.Sleep(500);
        }
        else
            break;
    };
}
