var tempFileName = Path.GetTempFileName();

await File.WriteAllTextAsync(tempFileName, "A temporary file with temporary data.");

var contents = await File.ReadAllTextAsync(tempFileName);
Console.WriteLine(contents);