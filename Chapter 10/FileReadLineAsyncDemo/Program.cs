using System.Text;

var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
var tempFileName = Path.GetTempFileName();
var stringBuilder = new StringBuilder();
for (var i = 0; i < 1000; i++)
{
    stringBuilder.AppendLine($"Line: {i}");
}

File.WriteAllText(tempFileName, stringBuilder.ToString());

try
{
    await foreach (var line in File.ReadLinesAsync(tempFileName, cts.Token))
    {
        await Task.Delay(1000, cts.Token);
        Console.WriteLine(line);
    }
}
catch (OperationCanceledException)
{
    Console.WriteLine("Operation cancelled.");
}
finally
{
    File.Delete(tempFileName);
}