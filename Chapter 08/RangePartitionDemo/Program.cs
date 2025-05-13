var source = Enumerable.Range(1, 1000000).ToArray();

var squares = (from number in source.AsParallel()
               select number * number).ToList();

Console.WriteLine("First 10 squares:");
for (int i = 0; i < 10; i++)
    Console.WriteLine(squares[i]);