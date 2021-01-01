using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;
using System;
using System.Threading.Tasks;

// This project specifies the serializer used to convert Lambda event into .NET classes in the project's main 
// main function. This assembly register a serializer for use when the project is being debugged using the
// AWS .NET Mock Lambda Test Tool.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace MyLambda
{
    public class Function
    {
        static int _main = 0;
        static  int _ctor = 0;

        /// <summary>
        /// The main entry point for the custom runtime.
        /// </summary>
        /// <param name="args"></param>
        private static async Task Main(string[] args)
        {
            System.Console.WriteLine("In main");
            _main++;

            Func<string, ILambdaContext, string> func = FunctionHandler;
            using(var handlerWrapper = HandlerWrapper.GetHandlerWrapper(func, new DefaultLambdaJsonSerializer()))
            using(var bootstrap = new LambdaBootstrap(handlerWrapper))
            {
                await bootstrap.RunAsync();
            }
        }

        public Function()
        {
            System.Console.WriteLine("In constructor");
            _ctor++;
        }

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        ///
        /// To use this handler to respond to an AWS event, reference the appropriate package from 
        /// https://github.com/aws/aws-lambda-dotnet#events
        /// and change the string input parameter to the desired event type.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string FunctionHandler(string input, ILambdaContext context)
        {
            System.Console.WriteLine("In handler");
            return $"{_main} {_ctor} {input?.ToUpper()}";
        }
    }
}
