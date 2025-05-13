
var task = new Task(() => Console.WriteLine("Hello world from Tasks!"));
task.Start();
task.Wait();
