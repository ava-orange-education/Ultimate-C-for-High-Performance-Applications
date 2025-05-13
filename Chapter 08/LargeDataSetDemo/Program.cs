using System.Diagnostics;

var source = Enumerable.Range(1, 1000000);

var middleAged = source.AsParallel()
    .Select(i => new Person { Id = i, Name = $"Person {i}", Age = i % 70 })
    .Where(person => person.Age <= 60 && person.Age > 39);

try
{
    var stopwatch = Stopwatch.StartNew();
    Console.WriteLine($"Number of middle age people = {middleAged.Count():N0}");
    Console.WriteLine($"Elapsed time: {stopwatch.ElapsedMilliseconds}");
}
catch (AggregateException ex)
{
    foreach (var exception in ex.InnerExceptions)
        Console.WriteLine(ex.Message);
}

class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}