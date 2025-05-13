using System;

public class Program
{
    delegate long FactorialDelegate(int n);

    public static void Main()
    {
        int number = 10;
        FactorialDelegate factorialDelegate = new FactorialDelegate(CalculateFactorial);
        IAsyncResult asyncResult = factorialDelegate.BeginInvoke(number, EndFactorialCallback, factorialDelegate);

        Console.WriteLine("Calculating factorial asynchronously...");

        asyncResult.AsyncWaitHandle.WaitOne();
    }

    public static long CalculateFactorial(int n)
    {
        long result = 1;
        for (int i = 1; i <= n; i++)
        {
            result *= i;
        }
        return result;
    }

    public static void EndFactorialCallback(IAsyncResult asyncResult)
    {
        FactorialDelegate factorialDelegate = (FactorialDelegate)asyncResult.AsyncState;

        long factorial = factorialDelegate.EndInvoke(asyncResult);

        Console.WriteLine("Factorial calculation complete. Result: " + factorial);
    }
}
