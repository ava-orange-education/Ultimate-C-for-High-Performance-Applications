using System.Collections.Concurrent;

var errors = new ConcurrentBag<string>();
var results = new ConcurrentDictionary<int, int>();

Parallel.For(1, 101, data =>
{
    var processed = false;
    while (!processed)
    {
        try
        {
            ProcessData(data);
            processed = true;
        }
        catch (InvalidOperationException ex)
        {
            errors.Add(ex.Message);
        }
    }
});

Console.WriteLine($"Square of 1 is {results[1]}");
Console.WriteLine($"Square of 100 is {results[100]}");
Console.WriteLine($"Errors: {errors.Count}");
Console.WriteLine($"Sample error: {errors.First()}");

void ProcessData(int data)
{
    if (Random.Shared.Next(0, 5) == 0)
        throw new InvalidOperationException($"Simulated failure for item {data}.");

    results.TryAdd(data, data * data);
}