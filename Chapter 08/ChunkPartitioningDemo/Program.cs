var source = Enumerable.Range(1, 1000000);

var roots = (from number in source.AsParallel()
             select Math.Sqrt(number)).ToList();

Console.WriteLine("First 10 roots:");
for (int i = 0; i < 10; i++)
    Console.WriteLine(roots[i]);