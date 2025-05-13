Task.Run(() => Console.WriteLine("Executing the first task."))
    .ContinueWith(task => Console.WriteLine("Executing continuation task."))
    .Wait();