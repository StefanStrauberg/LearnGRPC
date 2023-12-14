using System.Collections.Concurrent;
using Grpc.Core;

namespace GrpcHelloWorldServer.Services;

public class ChatRoom
{
    private ConcurrentDictionary<string, IServerStreamWriter<Protos.Message>> users 
        = new();

    public void Join(string name, IServerStreamWriter<Protos.Message> response) 
        => users.TryAdd(name, response);

    public void Remove(string name)  
        => users.TryRemove(name, out var s);

    public async Task BroadcastMessageAsync(Protos.Message message) 
        => await BroadcastMessages(message);

    private async Task BroadcastMessages(Protos.Message message)
    {
        foreach (var user in users.Where(x => x.Key != message.User))
        {
            var item = await SendMessageToSubscriber(user, message);
            if (item != null)
            {
                Remove(item?.Key);
            };
        }
    }

    private async Task<KeyValuePair<string, IServerStreamWriter<Protos.Message>>?> SendMessageToSubscriber(KeyValuePair<string, IServerStreamWriter<Protos.Message>> user, Protos.Message message)
    {
        try
        {
            await user.Value.WriteAsync(message);
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return user;
        }
    }
}