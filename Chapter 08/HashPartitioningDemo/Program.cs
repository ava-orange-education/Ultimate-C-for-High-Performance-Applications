var source = new[] {"apple","apricot",
    "avocado","blueberry","strawberry",
    "blackberry","raspberry" };

var groupedWords = from word in source.AsParallel()
                   group word by word[0] into wordGroup
                   select new { Key = wordGroup.Key, Words = wordGroup };

foreach (var group in groupedWords)
    Console.WriteLine(
        $"Words beginning with {group.Key}: {string.Join(", ", group.Words)}.");