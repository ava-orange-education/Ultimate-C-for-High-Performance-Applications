
var result = await GetSumAsync(10);
Console.WriteLine($"The synchronous sum is: {result}");

result = await Task.Run(() => GetSum(20));
Console.WriteLine($"The background sum result is: {result}");


static Task<int> GetSumAsync(int range)
{
    var sum = 0;
    for (var i = 0; i < range; i++)
    {
        sum += i;
    }
    return Task.FromResult(sum);
}

static int GetSum(int range)
{
    var sum = 0;
    for (var i = 0; i < range; i++)
    {
        sum += i;
    }
    return sum;
}