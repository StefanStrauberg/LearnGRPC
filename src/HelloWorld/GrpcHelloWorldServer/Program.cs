using GrpcHelloWorldServer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddSingleton<ChatRoom>();

var app = builder.Build();

app.MapGrpcService<HelloWorldService>();
app.Run();
