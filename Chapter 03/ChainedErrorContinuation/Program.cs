RunContinuation(10).Wait();
RunContinuation(0).Wait();

Task RunContinuation(int number)
{
    return Task.Run(() => ProcessData(number))
        .ContinueWith(task =>
        {
            if (task.IsCompletedSuccessfully)
                Console.WriteLine($"The result is: {task.Result}");
            else
                Console.WriteLine($"Task failed with message: {task.Exception?.Flatten().Message}");
        });
}

decimal ProcessData(int number)
{
    if (number == 0)
    {
        throw new ArgumentException("Invalid argument.");
    }

    return 100.0M / number;
}
