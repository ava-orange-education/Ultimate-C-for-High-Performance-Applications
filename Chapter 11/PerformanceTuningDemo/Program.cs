var maxWorkerThreads = 0;
var maxIoCompletionThreads = 0;
var minWorkerThreads = 0;
var minIoCompletionThreads = 0;

ThreadPool.GetMaxThreads(out maxWorkerThreads, out maxIoCompletionThreads);
ThreadPool.GetMinThreads(out minWorkerThreads, out minIoCompletionThreads);

Console.WriteLine($"Max worker: {maxWorkerThreads:N0}; max I/O: {maxIoCompletionThreads:N0}");
Console.WriteLine($"Min worker: {minWorkerThreads:N0}; min I/O: {minIoCompletionThreads:N0}");

ThreadPool.SetMinThreads(minWorkerThreads + 20, minIoCompletionThreads);
ThreadPool.SetMaxThreads(maxWorkerThreads, maxIoCompletionThreads + 10);

ThreadPool.GetMaxThreads(out maxWorkerThreads, out maxIoCompletionThreads);
ThreadPool.GetMinThreads(out minWorkerThreads, out minIoCompletionThreads);

Console.WriteLine($"Max worker: {maxWorkerThreads:N0}; max I/O: {maxIoCompletionThreads:N0}");
Console.WriteLine($"Min worker: {minWorkerThreads:N0}; min I/O: {minIoCompletionThreads:N0}");