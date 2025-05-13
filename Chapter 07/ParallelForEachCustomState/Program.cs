var source = new List<string>() { "One", "Two", "Three", "Four", "Five", "Six" };

Parallel.ForEach(source,
    localInit: () =>
    {
        // Initialize thread-local state
        return new List<string>();
    },
    body: (element, state, localState) =>
    {
        // Process the iteration and update thread-local state
        localState.Add(element);
        return localState;
    },
    localFinally: (finalState) =>
    {
        // Handle final thread-local state
        Console.WriteLine($"Thread completed with local state: {string.Join(", ", finalState)}");
    });
