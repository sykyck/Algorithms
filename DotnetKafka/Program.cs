using Confluent.Kafka;
using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        string broker = Environment.GetEnvironmentVariable("KAFKA_BROKER") ?? "localhost:9092";
        string topic = "test-topic";

        // Start consumer in background
        var cts = new CancellationTokenSource();
        Task.Run(() => StartConsumer(broker, topic, cts.Token));

        // Producer loop
        var config = new ProducerConfig { BootstrapServers = broker };
        using var producer = new ProducerBuilder<Null, string>(config).Build();

        Console.WriteLine("Enter messages to send to Kafka. Type 'exit' to quit.");
        string? input;
        while ((input = Console.ReadLine()) != null)
        {
            if (input.ToLower() == "exit") break;

            await producer.ProduceAsync(topic, new Message<Null, string> { Value = input });
            Console.WriteLine($"Produced: {input}");
        }

        cts.Cancel();
    }

    static void StartConsumer(string broker, string topic, CancellationToken token)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = broker,
            GroupId = "test-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe(topic);

        try
        {
            while (!token.IsCancellationRequested)
            {
                var cr = consumer.Consume(token);
                Console.WriteLine($"Consumed: {cr.Message.Value}");
            }
        }
        catch (OperationCanceledException) { }
        finally
        {
            consumer.Close();
        }
    }
}
