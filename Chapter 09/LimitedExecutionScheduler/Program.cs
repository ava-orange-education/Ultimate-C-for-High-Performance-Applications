using LimitedExecutionScheduler;

using var scheduler = new CustomLimitedScheduler(2);
for (var i = 0; i < 10; i++)
{
    _ = Task.Factory.StartNew(() =>
    {
        var threadId = Thread.CurrentThread.ManagedThreadId;
        var taskId = Task.CurrentId;
        Console.WriteLine($"Executing task {taskId} on thread: {threadId}");
    }, CancellationToken.None, TaskCreationOptions.None, scheduler);
}