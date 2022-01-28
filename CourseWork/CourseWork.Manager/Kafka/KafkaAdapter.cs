using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Confluent.Kafka;
using CourseWork.ProtoHelpers;
using CourseWork.Worker.Protobuf;

namespace CourseWork.Manager.Kafka
{
    public static class KafkaAdapter
    {
        public static async Task ProduceAsync(Guid guid, WorkerRq workerRq)
        {
            Results.TryAdd(guid, Channel.CreateBounded<WorkerRs>(1));
            await WorkerProducer.ProduceAsync(
                TopicRq,
                new Message<Null, WorkerRq>()
                {
                    Value = workerRq
                }
            );
        }

        private static ConcurrentDictionary<Guid, Channel<WorkerRs>> Results { get; } = new();

        public static IAsyncEnumerable<WorkerRs> Consume(Guid correlationId)
        {
            return Results[correlationId].Reader.ReadAllAsync();
        }

        private static IProducer<Null, WorkerRq> WorkerProducer  { get; set; }
        private static IConsumer<Null, WorkerRs> WorkerConsumer  { get; set; }
        private static Thread                    ConsumingThread { get; set; }

        private static string TopicRq { get; set; }

        private static string TopicRs { get; set; }

        public static void Initialize(
            string bootstrapServers,
            string topicRq,
            string topicRs,
            string groupId
        )
        {
            WorkerConsumer = new ConsumerBuilder<Null, WorkerRs>(
                    new ConsumerConfig()
                    {
                        BootstrapServers = bootstrapServers,
                        GroupId = groupId
                    }
                )
                .SetValueDeserializer(new ProtoDeserializer<WorkerRs>())
                .Build();
            WorkerConsumer.Subscribe(topicRs);
            ConsumingThread = new Thread(
                async () =>
                {
                    while (true)
                    {
                        var result = WorkerConsumer.Consume(TimeSpan.FromMilliseconds(1000));
                        if (result is null) continue;
                        var guid = Guid.Parse(result.Message.Value.CorrelationId);
                        var writer = Results[guid].Writer;
                        await writer.WriteAsync(result.Message.Value);
                        writer.Complete();
                        Results.TryRemove(guid, out _);
                    }
                }
            );
            ConsumingThread.IsBackground = true;
            ConsumingThread.Start();
            
            WorkerProducer = new ProducerBuilder<Null, WorkerRq>(
                    new ProducerConfig()
                    {
                        BootstrapServers = bootstrapServers
                    }
                )
                .SetValueSerializer(new ProtoSerializer<WorkerRq>())
                .Build();
            TopicRq = topicRq;
            TopicRs = topicRs;
        }
    }
}