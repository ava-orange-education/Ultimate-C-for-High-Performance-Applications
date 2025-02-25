bool createdNew = false;
Mutex singleInstanceMutex = new Mutex(true, "MutexDemo", out createdNew);
try
{
    if (createdNew)
    {
        Console.WriteLine("The application is running. Press any key to exit.");
        Console.ReadKey();
    }
    else
        Console.WriteLine("Another instance is running. Exiting now.");
}
finally
{
    singleInstanceMutex.Dispose();
}
