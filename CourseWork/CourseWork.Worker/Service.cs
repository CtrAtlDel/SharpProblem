using System;
using Confluent.Kafka;
using CourseWork.Worker.Protobuf;
using Serilog;

namespace CourseWork.Worker
{
    public static class Service
    {
        public static void Serve(
            string                    rqTopic,
            IConsumer<Null, WorkerRq> consumer,
            string                    rsTopic,
            IProducer<Null, WorkerRs> producer
        )
        {
            Log.Information("Subscribing on topic {RqTopic}", rqTopic);
            consumer.Subscribe(rqTopic);
            while (true)
            {
                var record = consumer.Consume(TimeSpan.FromMilliseconds(500));
                if (record is null) continue;
                Log.Information("Receive new message from request topic with corrid={CorrelationId}", record.Message.Value.CorrelationId);
                var rsData = Algorithms.Multiplication(record.Message.Value.RqData);
                var response = new WorkerRs
                {
                    RsData = rsData,
                    CorrelationId = record.Message.Value.CorrelationId
                };
                Log.Information("Sending result to response topic");
                producer.Produce(
                    rsTopic,
                    new Message<Null, WorkerRs>
                    {
                        Value = response
                    }
                );
            }
        }
    }
}