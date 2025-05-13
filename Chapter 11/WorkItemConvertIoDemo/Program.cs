var tempFile = Path.GetTempFileName();
try
{
    //Old work item
    ThreadPool.QueueUserWorkItem(fileName =>
    {
        File.AppendAllText(fileName, "Old work item.\n");
    }, tempFile, true);

    await Task.Delay(100);

    //New task
    await WriteLogAsync(tempFile, "New task.");
    Console.WriteLine(File.ReadAllText(tempFile));
}
finally
{
    File.Delete(tempFile);
}

static async Task WriteLogAsync(string logFilePath, string message)
{
    await File.AppendAllTextAsync(logFilePath, message);
}