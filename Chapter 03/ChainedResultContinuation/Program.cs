
int number = 7;
Task.Run(() => IsPrime(number))
    .ContinueWith(task => Console.WriteLine($"The number {(task.Result ? "is" : "is not")} prime."))
    .Wait();

bool IsPrime(int number)
{
    if (number <= 1)
    {
        return false;
    }

    for (int i = 2; i <= Math.Sqrt(number); i++)
    {
        if (number % i == 0)
        {
            return false;
        }
    }

    return true;
}
