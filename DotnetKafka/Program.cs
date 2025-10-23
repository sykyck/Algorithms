using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;

class Program
{
    private const string Topic = "demo-topic";

    static async Task Main()
    {
        var broker = Environment.GetEnvironmentVariable("KAFKA_BROKER") ?? "kafka:29092";

        var producerConfig = new ProducerConfig { BootstrapServers = broker };
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = broker,
            GroupId = "dotnet-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = true
        };

        Console.WriteLine("Starting consumer in background...");
        var cts = new CancellationTokenSource();

        // Run consumer in background
        _ = Task.Run(() => RunConsumer(consumerConfig, cts.Token));

        // Producer runs in foreground
        using var producer = new ProducerBuilder<Null, string>(producerConfig).Build();
        Console.WriteLine("Enter messages to send to Kafka. Type 'exit' to quit.");

        while (true)
        {
            var message = Console.ReadLine();
            if (message?.ToLower() == "exit") break;

            await producer.ProduceAsync(Topic, new Message<Null, string> { Value = message });
            Console.WriteLine($"Produced: {message}");
        }

        cts.Cancel();
        Console.WriteLine("Shutting down...");
    }

    private static void RunConsumer(ConsumerConfig config, CancellationToken token)
    {
        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe(Topic);

        try
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var cr = consumer.Consume(token);
                    if (cr != null)
                        Console.WriteLine($"Consumed: {cr.Message.Value}");
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"Consume error: {e.Error.Reason}");
                }
            }
        }
        catch (OperationCanceledException)
        {
            consumer.Close();
        }
    }
}
