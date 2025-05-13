using System.Diagnostics;

object lockObject = new object();
var tempFileName = Path.GetTempFileName();
try
{
    var data = LoadData(100_000);
    var stopwatch = Stopwatch.StartNew();

    Parallel.ForEach(data, record =>
    {
        string result = PerformComputation(record);
        lock (lockObject)
        {
            File.AppendAllText(tempFileName, result + Environment.NewLine);
        }
    });

    stopwatch.Stop();
    Console.WriteLine($"Processing time (inefficient): {stopwatch.ElapsedMilliseconds} ms");
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


    //amount = principal * Math.Pow(1 + rate, periods);
    for (int i = 0; i < periods; i++)
    {
        amount *= (1 + rate);
    }

    return $"{input.ToUpper()} - Final Amount: {amount:F2}";
}

