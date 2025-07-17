using JasperFx.Events;
using Marten.Events.Aggregation;
using SharedContracts;


namespace eCommerce.Data
{
    public record Product(Guid Id, string Name, string Description, string Category, decimal Price, int InventoryLevel, string ImageUrl, Guid VendorId);

    public class ProductProjection : SingleStreamProjection<Product, Guid>
    {
        public Product Create(IEvent<Events.ProductCreated> created)
        {
            return new Product(created.StreamId, created.Data.Name, created.Data.Description, created.Data.Category, created.Data.Price, created.Data.InventoryLevel, created.Data.ImageUrl, created.Data.VendorId);
        }

    }
}

/*
 {
  "name": "boots",
  "price": 2.24,
  "description": "made for walking",
  "category": "footwear",
  "imageUrl": "",
  "vendorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}

*/