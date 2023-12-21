using Microsoft.EntityFrameworkCore;
using ProductGrpc.Data;
using ProductGrpc.Services;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddDbContext<ProductContext>(options =>
    options.UseInMemoryDatabase("Products"));

var app = builder.Build();
SeedDatabase(app);

static void SeedDatabase(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var service = scope.ServiceProvider;
    var productContext = service.GetService<ProductContext>();
    var logger = service.GetService<ILogger<Program>>();
    if (productContext is not null && logger is not null)
    {
        ProductContextSeed.Seed(productContext);
        logger.LogInformation("Database is seeded with test data.");
    }
}

app.MapGrpcService<ProductService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
