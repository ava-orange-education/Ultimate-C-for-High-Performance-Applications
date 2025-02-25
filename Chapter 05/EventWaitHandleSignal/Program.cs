using (var autoResetEvent = new EventWaitHandle(false, EventResetMode.AutoReset, "EventWaitDemo"))
{
    Console.WriteLine("Press any key to signal waiter process.");
    Console.ReadKey();
    autoResetEvent.Set();
    Console.WriteLine("Event set.");
}