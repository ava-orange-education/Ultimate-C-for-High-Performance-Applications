Task resultTask = null!;
try
{
    resultTask = Task.WhenAll(
        ThrowErrorAsync(),
        ThrowErrorAsync(),
        ThrowErrorAsync(),
        Task.Delay(500)
        );
    await resultTask;
}
catch (Exception)
{
    Console.WriteLine($"Caught: {resultTask.Exception?.Flatten().Message}");
}

List<Task> tasks = [.. new[] {ThrowErrorAsync(),
    ThrowErrorAsync(),
    ThrowErrorAsync(),
    Task.Delay(500)}];

Task<Task> whenAnyTask = null!;
try
{
    whenAnyTask = Task.WhenAny(tasks);
    await whenAnyTask.Unwrap();
}
catch (Exception ex)
{
    Console.WriteLine($"Caught: {ex.Message}");
}

var remainingTasks = tasks.Where(t => t != whenAnyTask.Result).ToList();
await Task.WhenAll(remainingTasks).ContinueWith(task =>
{
    foreach (Task remainingTask in remainingTasks)
    {
        if (remainingTask.IsFaulted)
        {
            Console.WriteLine($"Caught: {remainingTask.Exception?.Flatten().Message}");
        }
    }
});

static async Task ThrowErrorAsync()
{
    throw new ArgumentException("Invalid argument provided.");
}