using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseWork.Manager.Kafka;
using CourseWork.Manager.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CourseWork.Manager
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services) { services.AddGrpc(); }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapGrpcService<MatrixMulServiceImpl>();
                    endpoints.MapGet(
                        "/",
                        async context =>
                        {
                            await context.Response.WriteAsync(
                                "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909"
                            );
                        }
                    );
                }
            );

            KafkaAdapter.Initialize(
                configuration["MSS_KAFKA_BOOTSTRAP_SERVERS"],
                configuration["MSS_TOPIC_RQ"] ?? "mss.worker.rq",
                configuration["MSS_TOPIC_RS"] ?? "mss.worker.rs",
                configuration["MSS_KAFKA_GROUP_ID"] ?? "mss.manager"
            );
        }
    }
}