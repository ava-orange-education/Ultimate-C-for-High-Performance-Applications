using System;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;

using var loggerFactory = LoggerFactory.Create(builder =>
    builder.AddSimpleConsole().SetMinimumLevel(LogLevel.Information));
var logger = loggerFactory.CreateLogger("TransientRetry");

var results = Enumerable.Range(1, 20)
    .AsParallel()
    .Select(i => Retry(() => Simulate(i), logger, maxRetries: 3, baseDelayMs: 100))
    .ToList();

results.ForEach(Console.WriteLine);

string Simulate(int value)
{
    if (value % 7 == 0 && Random.Shared.NextDouble() < 0.5)
        throw new InvalidOperationException($"Transient failure for {value}");
    return $"Processed {value}";
}

T Retry<T>(Func<T> action, ILogger logger, int maxRetries, int baseDelayMs)
{
    for (int attempt = 0; ; attempt++)
    {
        try { return action(); }
        catch (Exception ex) when (attempt < maxRetries)
        {
            int delay = baseDelayMs * (1 << attempt);
            logger.LogWarning(ex, "Attempt {Attempt} failed. Retrying in {Delay} ms...", attempt + 1, delay);
            Thread.Sleep(delay);
        }
        catch
        {
            logger.LogError("All {Max} attempts failed.", maxRetries);
            throw;
        }
    }
}
