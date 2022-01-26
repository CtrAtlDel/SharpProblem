using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging.Abstractions;

namespace CourseProject.Client
{
    internal static class Programm
    {
        public static async Task Main()
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            using var channel = GrpcChannel.ForAddress(
                "http://localhost:18081",
                new GrpcChannelOptions()
                {
                    Credentials = ChannelCredentials.Insecure,
                    LoggerFactory = new NullLoggerFactory()
                }
            );
            // var mss = new MatrixSumService.MatrixSumServiceClient(channel);
        }

        private static async Task Input()
        {
            //input Console or File
            var tasks = new Dictionary<int, Task>();
            await Task.WhenAll(tasks.Values);
        }
    }
};