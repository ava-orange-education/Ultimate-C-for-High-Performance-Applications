using System.Collections.Concurrent;

var source = Enumerable.Range(1, 1000000);
var partitioner = Partitioner.Create(source, EnumerablePartitionerOptions.NoBuffering);

var squares = (from number in partitioner.AsParallel()
               where number % 1000 == 0
               select (uint)(number * number)).ToList();

Console.WriteLine("First 10 squares:");
for (int i = 0; i < 10; i++)
    Console.WriteLine(squares[i]);