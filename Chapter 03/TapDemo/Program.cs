try
{
    Console.WriteLine("The 5th Fibonacci number is: " + FibonacciAsync(5).Result);
    Console.WriteLine("The 11th Fibonacci number is: " + FibonacciAsync(11).Result);
}
catch (AggregateException ex)
{
    foreach (var exception in ex.Flatten().InnerExceptions)
        Console.WriteLine(exception.Message);
}

Task<long> FibonacciAsync(int n)
{
    return Task.Run(() => CalculateFibonacci(n));
}

long CalculateFibonacci(int n)
{
    if (n <= 1)
    {
        return n;
    }

    return CalculateFibonacci(n - 1) + CalculateFibonacci(n - 2);
}
