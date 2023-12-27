using Grpc.Core;
using Grpc.Net.Client;
using ProductGrpcClient.Protos;
using static ProductGrpcClient.Protos.ProductProtoService;

using var channel = GrpcChannel.ForAddress("http://localhost:5005");
var client = new ProductProtoServiceClient(channel);

// GetProductAsync
await GetProductAsync(client);
// GetAllProductsAsync
await GetAllProductsAsync(client);

// AddProductAsync
Console.WriteLine(new string('-',50));
Console.WriteLine("AddProductAsync started..");

static async Task GetProductAsync(ProductProtoServiceClient client)
{
    Console.WriteLine(new string('-',50));
    Console.WriteLine("GetProductAsync started..");
    var getProductRequest = new GetProductRequest { ProductId = 1 }; 
    var response = await client.GetProductAsync(getProductRequest);
    Console.WriteLine(response);
}

static async Task GetAllProductsAsync(ProductProtoServiceClient client)
{
    Console.WriteLine(new string('-',50));
    Console.WriteLine("GetAllProductsAsync started..");
    var getAllProductsRequest = new GetAllProductsRequest();
    using var clientData = client.GetAllProducts(getAllProductsRequest);
    await foreach (var currentProduct in clientData.ResponseStream.ReadAllAsync())
        Console.WriteLine(currentProduct);
}