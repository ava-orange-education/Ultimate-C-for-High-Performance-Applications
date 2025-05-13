int total = 0;

Parallel.For(1, 1000, () => 0, (number, state, localValue) =>
{
    localValue = number * 5;
    return localValue;
}, localValue =>
{
    Interlocked.Add(ref total, localValue);
});

Console.WriteLine($"Total value: {total:N0}");