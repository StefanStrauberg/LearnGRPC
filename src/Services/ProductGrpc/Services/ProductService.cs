using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using ProductGrpc.Data;
using ProductGrpc.Protos;

namespace ProductGrpc.Services;

public class ProductService(ILogger<ProductService> logger, ProductContext context) : ProductProtoService.ProductProtoServiceBase
{
    readonly ILogger<ProductService> _logger = logger 
        ?? throw new ArgumentNullException(nameof(logger));
    readonly ProductContext _context = context 
        ?? throw new ArgumentNullException(nameof(context));

    public override Task<Empty> Test(Empty request, ServerCallContext context)
    {
        return base.Test(request, context);
    }

    public override async Task<ProductModel> GetProduct(GetProductRequest request, ServerCallContext context)
    {
        var product = await _context.Products.FindAsync(request.ProductId);
        
        if (product is null)
        {
            throw new ArgumentNullException(nameof(product));
        }

        var productModel = new ProductModel
        {
            ProductId = product.ProductId,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Status = ProductStatus.Instock,
            CreatedTime = Timestamp.FromDateTime(product.CreatedTime)
        };

        return productModel;
    }
}
