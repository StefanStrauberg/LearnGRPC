using Grpc.Core;
using GrpcHelloWorldServer.Protos;

namespace GrpcHelloWorldServer.Services;

public class HelloWorldService(ILogger<HelloWorldService> logger) : HelloService.HelloServiceBase
{
    readonly ILogger<HelloWorldService> _logger = logger
        ?? throw new ArgumentNullException(nameof(logger));

    public override Task<HelloResponse> SayHello(HelloRequest request, ServerCallContext context)
    {
        string resultMessage = $"Hello {request.Name}";
        var response = new HelloResponse
        {
            Message = resultMessage
        };

        return Task.FromResult(response);
    }
}
