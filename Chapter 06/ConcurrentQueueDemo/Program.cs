using System.Collections.Concurrent;

var cts = new CancellationTokenSource();
var queue = new ConcurrentQueue<string>();
var tasks = new List<Task>();

tasks.Add(Task.Run(() => QueueJobs(cts)));
tasks.Add(Task.Run(() => RunJobs(cts.Token)));
tasks.Add(Task.Run(() => RunJobs(cts.Token)));

Task.WaitAll(tasks);

Console.WriteLine("Jobs completed.");

void QueueJobs(CancellationTokenSource tokenSource)
{
    for (int i = 1; i < 11; i++)
    {
        queue.Enqueue($"Job {i}");
        Thread.Sleep(100);
    }
    tokenSource.Cancel();
}

void RunJobs(CancellationToken token)
{
    while (!token.IsCancellationRequested || !queue.IsEmpty)
    {
        if (queue.TryDequeue(out string jobName))
        {
            Console.WriteLine($"Running job: {jobName}");
            Thread.Sleep(500);
        }
    }
}