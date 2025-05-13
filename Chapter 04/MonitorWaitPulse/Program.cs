class BuildSystem
{
    private readonly Dictionary<string, TaskNode> _tasks = new();

    public BuildSystem()
    {
        // Define tasks and their dependencies
        _tasks["A"] = new TaskNode("A", new[] { "B", "C" });
        _tasks["B"] = new TaskNode("B", new[] { "D" });
        _tasks["C"] = new TaskNode("C", Array.Empty<string>());
        _tasks["D"] = new TaskNode("D", Array.Empty<string>());
    }

    public void StartBuild()
    {
        foreach (var task in _tasks.Values)
        {
            new Thread(() => RunTask(task)).Start();
        }
    }

    private void RunTask(TaskNode task)
    {
        task.WaitForDependencies();

        Console.WriteLine($"Task {task.Name} is starting.");
        Thread.Sleep(1000); // Simulate work
        Console.WriteLine($"Task {task.Name} is completed.");

        foreach (var dependentTask in _tasks.Values)
        {
            if (dependentTask.Dependents.Contains(task.Name))
                dependentTask.DependencyCompleted();
        }
    }

    class TaskNode
    {
        private readonly object _lockObject = new();
        private int _remainingDependencies;

        public string Name { get; }
        public List<string> Dependents { get; } = new();

        public TaskNode(string name, IEnumerable<string> dependencies)
        {
            Name = name;

            Dependents.AddRange(dependencies);
            _remainingDependencies = Dependents.Count;
        }

        public void WaitForDependencies()
        {
            Monitor.Enter(_lockObject);
            try
            {
                while (_remainingDependencies > 0)
                {
                    Monitor.Wait(_lockObject); // Wait for all dependencies
                }
            }
            finally { Monitor.Exit(_lockObject); }
        }

        public void DependencyCompleted()
        {
            Monitor.Enter(_lockObject);
            try
            {
                _remainingDependencies--;
                if (_remainingDependencies == 0)
                {
                    Monitor.Pulse(_lockObject); // Wake waiting threads
                }
            }
            finally { Monitor.Exit(_lockObject); }
        }
    }
}

class Program
{
    static void Main()
    {
        BuildSystem buildSystem = new();
        buildSystem.StartBuild();
    }
}
