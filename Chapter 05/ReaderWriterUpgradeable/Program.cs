var numberList = new List<int>();
var keyPressed = false;
var readerWriterLock = new ReaderWriterLockSlim();

try
{
    var addThread = new Thread(AddNumbers);
    var showThread = new Thread(ShowNumbers);
    addThread.Start();
    showThread.Start();

    Console.ReadKey();
    keyPressed = true;

    addThread.Join();
    showThread.Join();
}
finally
{
    readerWriterLock.Dispose();
}

void ShowNumbers()
{
    while (!keyPressed)
    {
        readerWriterLock.EnterReadLock();
        try
        {
            Console.WriteLine(string.Join(", ", numberList));
        }
        finally
        {
            readerWriterLock.ExitReadLock();
        }
        Thread.Sleep(1000);
    }
}

void AddNumbers()
{
    int number = 0;
    while (!keyPressed)
    {
        if (number == 10)
            number = 0;
        AddIfNotExist(number);
        number++;
        Thread.Sleep(1000);
    }
}

void AddIfNotExist(int number)
{
    readerWriterLock.EnterUpgradeableReadLock();
    try
    {
        if (!numberList.Contains(number))
        {
            readerWriterLock.EnterWriteLock();
            try
            {
                numberList.Add(number);
            }
            finally
            {
                readerWriterLock.ExitWriteLock();
            }
        }
    }
    finally
    {
        readerWriterLock.ExitUpgradeableReadLock();
    }
}