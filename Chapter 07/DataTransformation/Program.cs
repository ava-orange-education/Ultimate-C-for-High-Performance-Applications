using System.Collections.Concurrent;

namespace DataTransformation;

internal class Program
{
    static void Main(string[] args)
    {
        Console.ReadKey();
        var source = new DataSource();
        var dataStore = new DataStore();
        var dataLog = new ConcurrentBag<string>();

        Parallel.ForEach(source.GetData(), data =>
        {
            try
            {
                StandardData standardData = new StandardData(data);
                dataStore.StoreData(standardData);
            }
            catch (Exception ex)
            {
                dataLog.Add(ex.ToString());
            }
        });

        Console.WriteLine("Transformation complete. Sample records:\n");

        foreach (var data in dataStore.GetElements(5))
            Console.WriteLine($"{data.LastName}, {data.FirstName} {data.MiddleName}");

        Console.WriteLine($"\n{dataLog.Count} errors.");
        Console.ReadKey();
    }
}
