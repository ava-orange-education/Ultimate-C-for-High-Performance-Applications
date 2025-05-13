
var ctsParent = new CancellationTokenSource();
var ctsChild = CancellationTokenSource.CreateLinkedTokenSource(ctsParent.Token);

var task1 = Task.Run(() => DoWork(ctsParent.Token, "one"), ctsParent.Token);

var task2 = Task.Run(() => DoWork(ctsChild.Token, "two"), ctsChild.Token);

Thread.Sleep(3000);
ctsParent.Cancel();
task1.Wait();
task2.Wait();
ctsParent.Dispose();
ctsChild.Dispose();

static void DoWork(CancellationToken token, string taskName)
{
    int count = 0;
    while (!token.IsCancellationRequested)
    {
        count++;
        Thread.Sleep(1000);
    }

    Console.WriteLine($"Task {taskName} canceled after count of {count}");
}
