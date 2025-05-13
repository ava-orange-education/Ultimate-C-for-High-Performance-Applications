var source = Enumerable.Range(1, 100);

var tens = from num in source
           where num % 10 == 0
           select num;

foreach (var number in tens)
    Console.WriteLine(number);