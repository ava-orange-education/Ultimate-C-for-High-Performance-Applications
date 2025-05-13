var source = Enumerable.Range(1, 100);

(from number in source.AsParallel()
 where number % 10 == 0
 select number)
.ForAll(num =>
 Console.WriteLine($"Processing {num} on thread {Thread.CurrentThread.ManagedThreadId}."));