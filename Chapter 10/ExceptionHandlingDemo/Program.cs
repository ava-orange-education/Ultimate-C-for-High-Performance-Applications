
try
{
    ThrowErrorAsync().Wait();
}
catch (AggregateException ex)
{
    Console.WriteLine($"Caught: {ex.GetBaseException().Message}");
}

try
{
    await ThrowErrorAsync();
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Caught: {ex.Message}");
}

static async Task ThrowErrorAsync()
{
    throw new ArgumentException("Invalid argument provided.");
}
