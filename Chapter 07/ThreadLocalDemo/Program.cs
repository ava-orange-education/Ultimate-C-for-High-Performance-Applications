var threadLocalData = new ThreadLocal<int>(true);
try
{
    Parallel.For(1, 1000, number =>
    {
        threadLocalData.Value = number * 5;
    });

    int finalValue = threadLocalData.Values.Sum();
    Console.WriteLine($"The final value is {finalValue:N0}.");
    Console.WriteLine($"Number of threads: {threadLocalData.Values.Count}.");
}
finally
{
    threadLocalData.Dispose();
}