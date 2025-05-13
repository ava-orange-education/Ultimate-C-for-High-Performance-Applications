var multiplier = new Multiplier { Factor = 10 };
var amount = 10;
var done = new ManualResetEventSlim(); // Ensure completion

ThreadPool.QueueUserWorkItem(
    (int amount) =>
    {
        multiplier.Multiply(amount);
        done.Set(); // Signal completion
    }, amount, false);

done.Wait(); // Wait for Multiply to finish

Console.WriteLine(
    $"The result of {amount} * {multiplier.Factor} is {multiplier.Product}");

public class Multiplier
{
    public int Factor { get; set; }
    public int Product { get; set; }

    public void Multiply(int amount)
    {
        Product = amount * Factor;
    }
}
