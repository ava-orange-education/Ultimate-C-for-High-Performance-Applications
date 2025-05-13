var taskList = new List<Task>();
taskList.Add(Task.Run(() => Counter(10)));
taskList.Add(Task.Run(() => Counter(5)));

int completedIndex = Task.WaitAny(taskList.ToArray());
Console.WriteLine($"Finished quickest task {taskList[completedIndex].Id}.");

void Counter(int limit)
{
    for (int i = 0; i < limit; i++)
        Thread.Sleep(100);
    Console.WriteLine("Counted to " + limit);
}