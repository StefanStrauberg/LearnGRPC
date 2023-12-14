using Grpc.Core;
using GrpcHelloWorldServer.Protos;

namespace GrpcHelloWorldServer.Services;

public class HelloWorldService(ChatRoom chatroomService) : HelloService.HelloServiceBase
{
    readonly ChatRoom _chatroomService = chatroomService
        ?? throw new ArgumentNullException(nameof(chatroomService));

    public override async Task SayHello(IAsyncStreamReader<Message> requestStream, IServerStreamWriter<Message> responseStream, ServerCallContext context)
    {
        if (!await requestStream.MoveNext()) return;

        do
        {
            _chatroomService.Join(requestStream.Current.User, responseStream);
            await _chatroomService.BroadcastMessageAsync(requestStream.Current);
        } while (await requestStream.MoveNext());

        _chatroomService.Remove(context.Peer);

    }
}
