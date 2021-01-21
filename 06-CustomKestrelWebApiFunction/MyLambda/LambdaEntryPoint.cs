using Amazon.Lambda.AspNetCoreServer;
using Microsoft.AspNetCore.Hosting;

namespace MyLambda
{
    // See https://github.com/aws/aws-lambda-dotnet/tree/master/Libraries/src/Amazon.Lambda.AspNetCoreServer
    // The function handler, FunctionHandlerAsync, is in the base class. The fully qualified handler is
    // bootstrap::MyLambda.LambdaEntryPoint::FunctionHandlerAsync
    public class LambdaEntryPoint : ApplicationLoadBalancerFunction
    {
        protected override void Init(IWebHostBuilder builder)
        {
            System.Console.WriteLine("In LambdaEntryPoint.Init");

            builder.UseStartup<Startup>();
        }
    }
}
