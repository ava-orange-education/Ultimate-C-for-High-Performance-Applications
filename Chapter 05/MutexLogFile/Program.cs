var logFileName = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
    "MutexLogFile.txt");
using (var logFileMutex = new Mutex(false, "LogFileMutex"))
{
    Console.WriteLine("Writing to log file.");
    for (int i = 0; i < 60; i++)
    {
        logFileMutex.WaitOne();
        try
        {
            File.AppendAllText(logFileName,
                $"{Environment.ProcessId} - Logging instance {i}\n");
        }
        finally
        {
            logFileMutex.ReleaseMutex();
        }
        Console.Write(".");
        Thread.Sleep(500);
    }
    Console.WriteLine("\r\nWork finished.");
}
