using Grpc.Core;
using Grpc.Net.Client;
using ProductGrpcClient.Protos;

using var channel = GrpcChannel.ForAddress("http://localhost:5005");
var client = new ProductProtoService.ProductProtoServiceClient(channel);

// GetProductAsync
Console.WriteLine(new string('-',50));
Console.WriteLine("GetProductAsync started..");
var getProductRequest = new GetProductRequest { ProductId = 1 }; 
var response = await client.GetProductAsync(getProductRequest);
Console.WriteLine(response);

// GetAllProductsAsync
Console.WriteLine(new string('-',50));
Console.WriteLine("GetAllProductsAsync started..");
var getAllProductsRequest = new GetAllProductsRequest();
using (var clientData = client.GetAllProducts(getAllProductsRequest))
{
    await foreach (var currentProduct in clientData.ResponseStream.ReadAllAsync())
        Console.WriteLine(currentProduct);
}

// AddProductAsync
Console.WriteLine(new string('-',50));
Console.WriteLine("AddProductAsync started..");