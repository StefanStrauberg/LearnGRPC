using Grpc.Net.Client;
using GrpcHelloWorldClient.Protos;

using var channel = GrpcChannel.ForAddress("http://localhost:5000");
var client = new HelloService.HelloServiceClient(channel);

var reply = await client.SayHelloAsync(
        new HelloRequest { Name = "GRPC Client" }
    );

Console.WriteLine($"Greetings: {reply.Message}");