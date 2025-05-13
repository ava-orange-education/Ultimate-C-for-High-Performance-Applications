
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
FireAndForgetAsync().ContinueWith(task =>
{
    if (task.IsFaulted)
    {
        Console.WriteLine(task.Exception.Message);
    }
});
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

await Task.Delay(1000);
Console.WriteLine("Done");

static async Task FireAndForgetAsync()
{
    await Task.Delay(500);
    throw new InvalidOperationException("An invalid operation occurred.");
}