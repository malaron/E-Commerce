namespace ecommerce.Server.Services
{
    public static class ProductRequest
    {
        public record CreateProduct(string Name, decimal Price, string Description, string Category, string ImageUrl, int InventoryLevel, Guid VendorId);
        public record GetProduct(Guid Id);
    }
}
