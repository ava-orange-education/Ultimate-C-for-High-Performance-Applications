var source = Enumerable.Range(1, 100);

var sum = (from number in source.AsParallel()
           where number % 2 == 0
           select number)
           .Aggregate(0, (subtotal, num) => subtotal + num);

Console.WriteLine($"The sum of even numbers is: {sum}.");