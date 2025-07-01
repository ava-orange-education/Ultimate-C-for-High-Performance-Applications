using System.Collections.Concurrent;

var dictionary = new ConcurrentDictionary<string, int>();

//Add new entry
int addedValue = dictionary.AddOrUpdate("apple", 1, 
    (key, oldvalue) => oldvalue + 1);
Console.WriteLine($"apple: {addedValue}");

//Update existing entry
int updatedValue = dictionary.AddOrUpdate("apple", 1, 
    (key, oldvalue) => oldvalue + 1);
Console.WriteLine($"apple: {updatedValue}");