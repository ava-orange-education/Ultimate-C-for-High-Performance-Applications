var source = Enumerable.Range(1, 100000);

var filtered = from num in source.AsParallel().WithDegreeOfParallelism(2)
               where num % 10000 == 0
               select num;

foreach (var number in filtered)
    Console.WriteLine(number);