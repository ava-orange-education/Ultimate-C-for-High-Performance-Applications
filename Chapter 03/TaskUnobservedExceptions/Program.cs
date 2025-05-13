TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

RunTask();

Thread.Sleep(20);

GC.Collect();
GC.WaitForPendingFinalizers();
GC.Collect();

Console.ReadLine();

static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
{
    Console.WriteLine("Unobserved exception: " + e.Exception.ToString());
    e.SetObserved();
}

static void RunTask()
{
    Task.Run(() =>
    {
        Thread.Sleep(10);
        throw new InvalidOperationException("This will not be observed.");
    });
}