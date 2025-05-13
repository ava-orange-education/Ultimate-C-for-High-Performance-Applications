var source = new List<string>() { "One", "Two", "Three", "Four", "Five", "Six" };

Parallel.ForEach(source, number =>
{
    Console.WriteLine(number);
});