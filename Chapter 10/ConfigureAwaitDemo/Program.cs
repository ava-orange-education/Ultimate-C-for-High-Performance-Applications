Console.WriteLine("Starting...");

await Task.Delay(500); //Synchronizes
await Task.Delay(500).ConfigureAwait(false); //No syncronization

Console.WriteLine("Completed.");