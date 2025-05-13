//Old work item
var amount = 42;
ThreadPool.QueueUserWorkItem(state =>
{
    var square = amount * amount;
    Console.WriteLine(square);
}, amount, true);

await Task.Delay(100);

//New task
var result = await Task.Run(() => ComputeSquare(amount));
Console.WriteLine(result);

static int ComputeSquare(int amount)
{
    return amount * amount;
}