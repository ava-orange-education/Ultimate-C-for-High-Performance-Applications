int[,] matrix = new int[10000, 10000];

var sw = System.Diagnostics.Stopwatch.StartNew();
Parallel.For(0, matrix.GetLength(0), i =>
{
    for (int j = 0; j < matrix.GetLength(1); j++)
    {
        matrix[i, j] = i * j;
    }
});
Console.WriteLine($"Time taken: {sw.ElapsedMilliseconds} ms");

Console.WriteLine("Element at [3, 4]: " + matrix[3, 4]);
Console.WriteLine("Element at [100, 500]: " + matrix[100, 500]);