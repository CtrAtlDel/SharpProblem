using Confluent.Kafka;
using Serilog;

namespace CourseProject.Worker
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello im worker");
            Log.Logger = new LoggerConfiguration() //Create logger
                .MinimumLevel.Information()
                .WriteTo.Console().CreateLogger();
            Log.Information("Worker is start");
            var bootstrapServers = Environment.GetEnvironmentVariable("MSS_KAFKA_BOOTSTRAP_SERVERS") 
                                   ?? "localhost:29092";
            var groupId = Environment.GetEnvironmentVariable("MSS_KAFKA_GROUP_ID") ?? "mss.workers10";
            var rqTopic = Environment.GetEnvironmentVariable("MSS_KAFKA_TOPIC_RQ") ?? "mss.worker.rq";
            var rsTopic = Environment.GetEnvironmentVariable("MSS_KAFKA_TOPIC_RS") ?? "mss.worker.rs";
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Latest
            };
        //     using var consumer = new ConsumerBuilder<Null, WorkerRq>(consumerConfig)
        //         .SetValueDeserializer(new ProtoDeserializer<WorkerRq>())
        //         .Build();
        //
        //     var producerConfig = new ProducerConfig()
        //     {
        //         BootstrapServers = bootstrapServers
        //     };
        //     using var producer = new ProducerBuilder<Null, WorkerRs>(producerConfig)
        //         .SetValueSerializer(new ProtoSerializer<WorkerRs>())
        //         .Build();
        //     Log.Information("Serving...");
        //     Service.Serve(rqTopic, consumer, rsTopic, producer);
        //     Log.CloseAndFlush();
        }
    }
};

