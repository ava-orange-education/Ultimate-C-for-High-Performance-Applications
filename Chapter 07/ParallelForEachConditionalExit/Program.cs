var source = Enumerable.Range(1, 100);

Parallel.ForEach(source, (number, state, loopState) =>
{
    if (loopState > 9)
    {
        Console.WriteLine("Stopping loop...");
        state.Break();
    }

    if (state.LowestBreakIteration.HasValue && state.LowestBreakIteration.Value <= loopState)
    {
        Console.WriteLine($"Skipping index {loopState}.");
        return;
    }

    Console.WriteLine($"Processing index {loopState}.");
});