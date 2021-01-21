using Amazon.Lambda.ApplicationLoadBalancerEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLambda
{
    public class Program
    {
        public static void Main(string[] args)
        {
            System.Console.WriteLine("In Program.Main");
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("AWS_LAMBDA_FUNCTION_NAME")))
            {
                System.Console.WriteLine("Running under Kestrel");
                CreateHostBuilder(args).Build().Run();
            }
            else
            {
                System.Console.WriteLine("Running as zipped lambda package");
                var lambdaEntry = new LambdaEntryPoint();
                var functionHandler = (Func<ApplicationLoadBalancerRequest, ILambdaContext, Task<ApplicationLoadBalancerResponse>>)(lambdaEntry.FunctionHandlerAsync);
                using (var handlerWrapper = HandlerWrapper.GetHandlerWrapper(functionHandler, new DefaultLambdaJsonSerializer()))
                using (var bootstrap = new LambdaBootstrap(handlerWrapper))
                {
                    bootstrap.RunAsync().Wait();
                }
             }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
