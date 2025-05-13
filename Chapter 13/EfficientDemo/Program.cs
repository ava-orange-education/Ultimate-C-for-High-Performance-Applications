using System.Diagnostics;
using System.Text;
using System.Threading.Channels;

var tempFileName = Path.GetTempFileName();
try
{
    var data = LoadData(100_000);
    var stopwatch = Stopwatch.StartNew();
    var channel = Channel.CreateUnbounded<string>();
    var writerTask = WriteToFileAsync(channel);

    Parallel.ForEach(data, record =>
    {
        string result = PerformComputation(record);
        channel.Writer.TryWrite(result);
    });

    channel.Writer.Complete();
    await writerTask;

    stopwatch.Stop();
    Console.WriteLine($"Processing time (efficient): {stopwatch.ElapsedMilliseconds} ms");
}
finally
{
    File.Delete(tempFileName);
}

static string[] LoadData(int count)
{
    var data = new string[count];
    for (int i = 0; i < count; i++)
    {
        data[i] = $"Record {i}";
    }
    return data;
}

static string PerformComputation(string input)
{
    double principal = 1000;
    double rate = 0.05;
    int periods = 10000;
    double amount = principal;

    amount = principal * Math.Pow(1 + rate, periods);

    return $"{input.ToUpper()} - Final Amount: {amount:F2}";
}

static async Task WriteToFileAsync(Channel<string> channel)
{
    await using var writer = new StreamWriter("output.txt", append: true, encoding: Encoding.UTF8);
    await foreach (var line in channel.Reader.ReadAllAsync())
    {
        await writer.WriteLineAsync(line);
    }
}