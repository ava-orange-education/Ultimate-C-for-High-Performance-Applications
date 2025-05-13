using System.Collections.Concurrent;

var jobs = new ConcurrentStack<string>();
var cts = new CancellationTokenSource();
var tasks = new List<Task>();

tasks.Add(Task.Run(() => QueueJobs(cts)));
tasks.Add(Task.Run(() => RunJobs(cts.Token)));
tasks.Add(Task.Run(() => RunJobs(cts.Token)));

Task.WaitAll(tasks);

Console.WriteLine("Jobs complete.");

void QueueJobs(CancellationTokenSource source)
{
    for (int i = 1; i < 11; i++)
    {
        jobs.Push($"Job {i}");
        Thread.Sleep(100);
    }
    source.Cancel();
}

void RunJobs(CancellationToken token)
{
    while (!token.IsCancellationRequested || !jobs.IsEmpty)
    {
        if (jobs.TryPop(out string jobName))
        {
            Console.WriteLine($"Running job: {jobName}");
            Thread.Sleep(500);
        }
    }
}