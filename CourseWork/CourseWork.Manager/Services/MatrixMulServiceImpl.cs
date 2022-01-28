using System;
using System.Threading.Tasks;
using Grpc.Core;
using CourseWork.Manager.Kafka;
using CourseWork.Protobuf.Matrix;
using CourseWork.Worker.Protobuf;
using Serilog;

namespace CourseWork.Manager.Services
{
    public class MatrixMulServiceImpl : MatrixMulService.MatrixMulServiceBase
    {
        public override async Task<Matrix> Multiplication(PairMatrix request, ServerCallContext context)
        {
            Log.Information("Handling new request");
            var correlationId = Guid.NewGuid();
            Log.Information("Generated corrId: {CorrelationId}", correlationId);
            await KafkaAdapter.ProduceAsync(
                correlationId,
                new WorkerRq
                {
                    CorrelationId = correlationId.ToString(),
                    RqData = request
                }
            );
            Log.Information("Awaiting for result");
            await foreach (var result in KafkaAdapter.Consume(correlationId))
            {
                Log.Information("Receiving result");
                return result.RsData;
            }
            throw new Exception("Unreachable area");
        }
    }
}