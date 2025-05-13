using System.Collections.Concurrent;

var source = Enumerable.Range(1, 500);
var squares = new ConcurrentDictionary<int, int>();

Parallel.ForEach(source, number =>
{
    squares[number] = number * number;
});

Console.WriteLine($"2 squared is {squares[2]:N0}");
Console.WriteLine($"500 squared is {squares[500]:N0}");