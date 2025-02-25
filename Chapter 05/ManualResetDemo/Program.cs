ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();
var threadList = new List<Thread>();
bool paused = false;
var manualResetEvent = new ManualResetEvent(true);
try
{
    Console.WriteLine("Press space to pause.");
    for (int i = 0; i < 3; i++)
    {
        threadList.Add(new Thread(PlayLoop));
        threadList[i].Name = "Thread " + i;
        threadList[i].Start();
    }

    while (true)
    {
        if (paused)
            Console.WriteLine("Press space to resume. Press 'q' to quit.");
        keyInfo = Console.ReadKey();
        if (keyInfo.KeyChar == ' ')
        {
            paused = !paused;
            if (paused)
                manualResetEvent.Reset();
            else
                manualResetEvent.Set();
        }
        else if (keyInfo.KeyChar == 'q')
        {
            manualResetEvent.Set();
            break;
        }
    };
    foreach (var thread in threadList)
        thread.Join();
}
finally
{
    manualResetEvent.Dispose();
}

void PlayLoop()
{
    while (true)
    {
        manualResetEvent.WaitOne();
        if (keyInfo.KeyChar != 'q')
        {
            Console.WriteLine($"{Thread.CurrentThread.Name} working...");
            Thread.Sleep(500);
        }
        else
            break;
    };
}