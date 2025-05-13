var source = Enumerable.Range(1, 10000);

var tens = from num in source.AsParallel()
           where num % 1000 == 0
           select num;

foreach (var number in tens)
    Console.WriteLine(number);