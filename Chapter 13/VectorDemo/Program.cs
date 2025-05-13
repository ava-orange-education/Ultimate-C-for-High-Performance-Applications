var result = new double[100_000];
var source = new double[100_000];

//Setup
int i;
for (i = 0; i < source.Length; i++)
{
    source[i] = Random.Shared.NextDouble();
}

var stopwatch = System.Diagnostics.Stopwatch.StartNew();

//Standard Loop
for (i = 0; i < source.Length; i++)
{
    result[i] = source[i] * 10;
}

var standardLoopTime = stopwatch.Elapsed;
stopwatch.Restart();

//Vectorized Loop
var length = source.Length;
var vectorSize = System.Numerics.Vector<double>.Count;
for (i = 0; i < length - vectorSize; i += vectorSize)
{
    var vector = new System.Numerics.Vector<double>(source, i);
    (vector * 10).CopyTo(result, i);
}

//Process remaining elements
for (; i < length; i++)
{
    result[i] = source[i] * 10;
}

var vectorizedLoopTime = stopwatch.Elapsed;
stopwatch.Stop();
Console.WriteLine($"Standard Loop Time: {standardLoopTime}");
Console.WriteLine($"Vectorized Loop Time: {vectorizedLoopTime}");