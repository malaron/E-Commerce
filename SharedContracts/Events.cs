using Wolverine.Http;

namespace SharedContracts
{
    public class Events
    {
        public class VendorCreated
        {
            public Guid Id { get; set; } = Guid.CreateVersion7();
            public string Name { get; set; } = null!;
            public string PhoneNumber { get; set; } = null!;
            public string Address { get; set; } = null!;
            public string City { get; set; } = null!;
            public string Region { get; set; } = null!;
            public string Email { get; set; } = null!;
        }

        public record ProductCreated(Guid Id, string Name, string Description, string Category, decimal Price, int InventoryLevel, string ImageUrl, Guid VendorId);
        public record ProductRequested(Guid Id, string Name, string Description, string Category, decimal Price, int InventoryLevel, string ImageUrl, Guid VendorId);

        public record UserFirstNameUpdated(string FirstName);
        public record UserLastNameUpdated(string LastName);
        public record UserEmailUpdated(string Email);


    }
}
