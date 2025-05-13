var source = Enumerable.Range(1, 1000000);

var results = source.AsParallel()
                .Where(number => number % 100000 == 0)
                .AsSequential()
                .OrderByDescending(number => number)
                .Take(5);

foreach (var result in results)
    Console.WriteLine(result);