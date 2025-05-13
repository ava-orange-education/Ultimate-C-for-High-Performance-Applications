int[,] matrix = new int[1000, 1000];

Parallel.For(0, matrix.GetLength(0), i =>
{
    for (int j = 0; j < matrix.GetLength(1); j++)
    {
        matrix[i, j] = i * j;
    }
});

Console.WriteLine("Element at [3, 4]: " + matrix[3, 4]);
Console.WriteLine("Element at [100, 500]: " + matrix[100, 500]);