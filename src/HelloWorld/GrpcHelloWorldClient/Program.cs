using Grpc.Net.Client;
using GrpcHelloWorldClient.Protos;

var channel = GrpcChannel.ForAddress("http://localhost:5000");
var client = new HelloService.HelloServiceClient(channel);

Console.Write("Enter your name: ");
var userName = Console.ReadLine();
Console.WriteLine("For exit enter: bye");
using (var chat = client.SayHello())
{
    _ = Task.Run(async () =>
    {
        while (await chat.ResponseStream.MoveNext(cancellationToken: CancellationToken.None))
        {
            var response = chat.ResponseStream.Current;
            Console.WriteLine($"{response.User}: {response.Text}");
        }
    });

    await chat.RequestStream.WriteAsync(new Message { User = userName, Text = $"{userName} has joined the room" });
    
    string? line;
    while ((line = Console.ReadLine()) != null)
    {
        if (line?.ToLower() == "bye")
        {
            break;
        }
        await chat.RequestStream.WriteAsync(new Message { User = userName, Text = line });
    }
    await chat.RequestStream.CompleteAsync();
}
Console.WriteLine("Disconnecting");
await channel.ShutdownAsync();