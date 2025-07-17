using eCommerce.Data;
using Marten;
using static ecommerce.Server.Services.ProductRequest;
using static SharedContracts.Events;

namespace ecommerce.Server.Services
{
    public class GetProductHandler
    {
        public async Task<ProductRequested> Handle(Guid query, IDocumentSession session)
        {
            Product? product = session.Query<Product>().Where(p => p.Id == query).FirstOrDefault() 
                ?? throw new ProductNotFoundException("Product not found");

            ProductRequested productRequested = 
                new(product.Id, product.Name, product.Description, product.Category, product.Price, product.InventoryLevel, product.ImageUrl, product.VendorId);
            return productRequested;
        }
    }

    [Serializable]
    internal class ProductNotFoundException : Exception
    {
        public ProductNotFoundException()
        {
        }

        public ProductNotFoundException(string? message) : base(message)
        {
        }

        public ProductNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
