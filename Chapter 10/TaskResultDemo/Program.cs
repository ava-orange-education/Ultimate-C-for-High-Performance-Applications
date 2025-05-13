using System.Collections.Concurrent;

var cache = new ConcurrentDictionary<string, string>();
var tempFile = Path.GetTempFileName();
try
{
    await File.WriteAllTextAsync(tempFile, "This is some important cached data.");

    for (var i = 0; i < 10; i++)
    {
        Console.WriteLine(await GetFileData(tempFile));
    }
}
finally
{
    File.Delete(tempFile);
}

async Task<string> GetFileData(string tempFile)
{
    if (cache.TryGetValue(tempFile, out var result))
    {
        return result;
    }

    result = await File.ReadAllTextAsync(tempFile);
    cache.TryAdd(tempFile, result);
    return result;
}