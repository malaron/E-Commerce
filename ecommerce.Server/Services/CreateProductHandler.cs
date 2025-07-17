using eCommerce.Data;
using Marten;
using static ecommerce.Server.Services.ProductRequest;
using static SharedContracts.Events;

namespace ecommerce.Server.Services
{
    public class CreateProductHandler
    {
        public async Task<ProductCreated> Handle(CreateProduct command, IDocumentSession session)
        {
            var product = new ProductCreated(Guid.CreateVersion7(), command.Name, command.Description, command.Category, command.Price,
            command.InventoryLevel, command.ImageUrl, command.VendorId);

            session.Events.StartStream<Product>(product.Id, product);
            await session.SaveChangesAsync();

            return product;
        }
    }
}
