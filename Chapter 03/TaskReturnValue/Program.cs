
var task = new Task<long>(() => CalculateFibonacci(10));
task.Start();

Console.WriteLine("Calculating fibonacci...");
var result = task.Result;
Console.WriteLine("The 10th fibonacci number is " + result);

long CalculateFibonacci(int n)
{
    if (n <= 1)
    {
        return n;
    }

    return CalculateFibonacci(n - 1) + CalculateFibonacci(n - 2);
}
