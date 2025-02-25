var temperatures = new List<int>();
var sensor = new Random();
var averageTemp = 0.0;
var keyPressed = false;
var anomalyDetected = false;
var readerWriterLock = new ReaderWriterLockSlim();
try
{
    var threadList = new List<Thread>
{
    new Thread(GetSensorData),
    new Thread(RemoveOldData),
    new Thread(GetAverageTemp),
    new Thread(DetectAnomalies),
    new Thread(UpdateDisplay)
};
    foreach (var thread in threadList)
        thread.Start();

    Console.ReadKey();
    keyPressed = true;
    foreach (var thread in threadList)
        thread.Join();

}
finally
{
    readerWriterLock.Dispose();
}
void UpdateDisplay()
{
    while (!keyPressed)
    {
        Console.Write($"Average temperature: {averageTemp}");
        if (anomalyDetected)
            Console.WriteLine(" - Significant temperature change detected.");
        else
            Console.WriteLine(" - Temperature changes normal.");
        Thread.Sleep(1000);
    }
}


void GetSensorData()
{
    while (!keyPressed)
    {
        var sensorData = sensor.Next(15, 25);
        readerWriterLock.EnterWriteLock();
        try
        {
            temperatures.Add(sensorData);
        }
        finally
        {
            readerWriterLock.ExitWriteLock();
        }
        Thread.Sleep(100);
    }
}

void RemoveOldData()
{
    while (!keyPressed)
    {
        readerWriterLock.EnterWriteLock();
        try
        {
            while (temperatures.Count > 10)
                temperatures.RemoveAt(0);
        }
        finally
        {
            readerWriterLock.ExitWriteLock();
        }
        Thread.Sleep(100);
    }
}

void GetAverageTemp()
{
    while (!keyPressed)
    {
        readerWriterLock.EnterReadLock();
        try
        {
            Interlocked.Exchange(ref averageTemp, Math.Round(temperatures.Average(), 1));
        }
        finally
        {
            readerWriterLock.ExitReadLock();
        }
        Thread.Sleep(50);
    }
}

void DetectAnomalies()
{
    while (!keyPressed)
    {
        readerWriterLock.EnterReadLock();
        try
        {
            var differences = temperatures.Zip(temperatures.Skip(1), (x, y) => y - x).ToArray();
            anomalyDetected = differences.Any(x => Math.Abs(x) > 7);
        }
        finally
        {
            readerWriterLock.ExitReadLock();
        }
        Thread.Sleep(50);
    }
}