﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using CourseWork.Protobuf.Matrix;
using Microsoft.Extensions.Logging.Abstractions;


namespace CourseWork.Client
{
    internal static class Program
    {
       private static async Task Main(string[] args)
        {
            //var pair = InputConsole.InputMatrix();
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            using var channel = GrpcChannel.ForAddress(
                "http://localhost:18081",
                new GrpcChannelOptions()
                {
                    Credentials = ChannelCredentials.Insecure,
                    LoggerFactory = new NullLoggerFactory()
                }
            );
            var mss = new MatrixMulService.MatrixMulServiceClient(channel);
             //await Simple(mss);
             //await Several(mss);
            await PersonInput(mss);
        }

        private static async Task Simple(MatrixMulService.MatrixMulServiceClient mss)
        {
            var left = new double[3, 3] {{1, 1, 1}, {1, 6, 1}, {1, 2, 3}};
            var right = new double[3, 3] {{2, 2, 6}, {1, 1, 1}, {1, 2, 3}};

            var result = await mss.MultiplicationAsync(
                new PairMatrix()
                {
                    Left = Mapper.Map(left),
                    Right = Mapper.Map(right)
                }
            );

            Console.WriteLine(result.ToString());
        }

        private static async Task Several(MatrixMulService.MatrixMulServiceClient mss)
        {
            var stopwatch = new Stopwatch();
            var tasks = new Dictionary<int, Task>();
            for (var i = 0; i < 100; i++)
            {
                var left = Generator.Gen(100, 150);
                var right = Generator.Gen(100, 150);
                var task = mss.MultiplicationAsync(
                    new PairMatrix()
                    {
                        Left = Mapper.Map(left),
                        Right = Mapper.Map(right)
                    }
                );
                tasks[i] = task.ResponseAsync;
            }

            stopwatch.Start();
            await Task.WhenAll(tasks.Values);
            Console.WriteLine($"Elapsed time: {stopwatch.Elapsed}");
        }

        private static async Task PersonInput(MatrixMulService.MatrixMulServiceClient mss)
        {
            var stopwatch = new Stopwatch();

            var pair = InputConsole.InputMatrix();

            var result = await mss.MultiplicationAsync(
                new PairMatrix()
                {
                    Left = Mapper.Map(pair.First),
                    Right = Mapper.Map(pair.Second)
                }
            );

            Console.WriteLine("Result matrix:");
            for (int i = 0; i < result.DimX; i++)
            {
                for (int j = 0; j < result.DimX; j++)
                {
                    Console.Write(result.Lines[i].Columns[j] + "\t");
                }
                Console.WriteLine();
            }

            Console.WriteLine($"Elapsed time: {stopwatch.Elapsed}");
        }
    }
}