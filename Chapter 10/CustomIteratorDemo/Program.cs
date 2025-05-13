await foreach (var num in GenerateNumbersAsync(10))
{
    Console.WriteLine(num);
}

static async IAsyncEnumerable<int> GenerateNumbersAsync(int limit)
{
    for (var i = 0; i < limit; i++)
    {
        await Task.Delay(100);
        yield return Random.Shared.Next(100);
    }
}