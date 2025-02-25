using (var autoResetEvent = new EventWaitHandle(false, EventResetMode.AutoReset, "EventWaitDemo"))
{
    Console.WriteLine("Waiting for signal.");
    autoResetEvent.WaitOne();
    Console.WriteLine("Event set.");
}