using Marten;
using Marten.Events.Aggregation;
using SharedContracts;

namespace eCommerce.Data
{
    public class Vendor
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Region { get; set; } = null!;
        public string Email { get; set; } = null!;

        public void Apply(Events.VendorCreated @event)
        {
            Id = @event.Id;
            Name = @event.Name;
            PhoneNumber = @event.PhoneNumber;
            Address = @event.Address;
            City = @event.City;
            Region = @event.Region;
            Email = @event.Email;
        }
    }


    public class VendorProjection : SingleStreamProjection<Vendor, Guid>
    {

    }
}
