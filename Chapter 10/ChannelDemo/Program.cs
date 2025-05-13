using System.Threading.Channels;

var cts = new CancellationTokenSource();
var channel = Channel.CreateBounded<int>(5);


var producers = new List<Task>
        {
            ProduceAsync(channel.Writer, 1, cts.Token),
            ProduceAsync(channel.Writer, 2, cts.Token)
        };

var consumers = new List<Task>
        {
            ConsumeAsync(channel.Reader, 1, cts.Token),
            ConsumeAsync(channel.Reader, 2, cts.Token)
        };

await Task.Delay(3000);
cts.Cancel();

await Task.WhenAll(producers);
channel.Writer.Complete();
await Task.WhenAll(consumers);

static async Task ProduceAsync(ChannelWriter<int> writer, int producerId, CancellationToken token)
{
    try
    {
        while (!token.IsCancellationRequested)
        {
            var item = Random.Shared.Next(100);
            await writer.WriteAsync(item, token);
            Console.WriteLine($"Producer {producerId} produced: {item}");
            await Task.Delay(Random.Shared.Next(500, 1000), token);
        }
    }
    catch (OperationCanceledException) { }
    finally
    {
        Console.WriteLine($"Producer {producerId} stopped.");
    }
}

static async Task ConsumeAsync(ChannelReader<int> reader, int consumerId, CancellationToken token)
{
    try
    {
        await foreach (var item in reader.ReadAllAsync(token))
        {
            Console.WriteLine($"Consumer {consumerId} consumed: {item}");
            await Task.Delay(700, token);
        }
    }
    catch (OperationCanceledException) { }
    finally
    {
        Console.WriteLine($"Consumer {consumerId} stopped.");
    }
}