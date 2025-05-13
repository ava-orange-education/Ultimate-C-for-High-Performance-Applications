var source = Enumerable.Range(1, 100).ToArray();

var results = source.AsParallel()
              .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
              .Where(number => number % 10 == 0)
              .Select(number => number * number);

foreach (var result in results)
    Console.WriteLine(result);