
int number = 7;
var task = Task.Factory.StartNew(() => IsPrime(number));
Console.WriteLine("Checking prime asynchronously...");
bool result = task.Result;
Console.WriteLine($"The number {number} {(result ? "is" : "is not")} prime.");

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
