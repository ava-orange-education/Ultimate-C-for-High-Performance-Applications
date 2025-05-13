Task.Factory.StartNew(() =>
{
    while (true)
    {
        Thread.Sleep(1000);
        Console.WriteLine("Processed work iteration.");
    }
}, TaskCreationOptions.LongRunning);

Console.WriteLine("Press a key to exit.");
Console.ReadKey();