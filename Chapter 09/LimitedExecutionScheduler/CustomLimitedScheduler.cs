using System.Collections.Concurrent;

namespace LimitedExecutionScheduler
{
    internal class CustomLimitedScheduler : TaskScheduler, IDisposable
    {
        private volatile bool threadsStarted;
        private readonly int maxThreadPoolSize;
        private readonly ConcurrentBag<Thread> threadPool = [];
        private readonly BlockingCollection<Task> taskQueue = [];

        public CustomLimitedScheduler(int maxThreadPoolSize)
        {
            this.maxThreadPoolSize = maxThreadPoolSize;
        }

        public void Dispose()
        {
            this.taskQueue.CompleteAdding();
            foreach (Thread thread in this.threadPool)
            {
                thread.Join();
            }

            this.threadPool.Clear();
        }

        protected override IEnumerable<Task>? GetScheduledTasks()
        {
            return this.taskQueue.IsCompleted ? Enumerable.Empty<Task>() : this.taskQueue.ToArray();
        }

        protected override void QueueTask(Task task)
        {
            if (!this.taskQueue.IsAddingCompleted)
            {
                this.taskQueue.TryAdd(task);
                if (!this.threadsStarted)
                {
                    if (Interlocked.CompareExchange(ref this.threadsStarted, true, false) == false)
                    {
                        StartThreads();
                    }
                }
            }
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return !taskWasPreviouslyQueued && TryExecuteTask(task);
        }

        private void StartThreads()
        {
            for (var i = 0; i < this.maxThreadPoolSize; i++)
            {
                var thread = new Thread(ExecuteTasks);
                this.threadPool.Add(thread);
                thread.Start();
            }
        }

        private void ExecuteTasks()
        {
            while (!this.taskQueue.IsCompleted)
            {
                if (this.taskQueue.TryTake(out Task? task))
                {
                    TryExecuteTask(task);
                }
            }
        }
    }
}
