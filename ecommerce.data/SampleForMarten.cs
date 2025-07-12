using Marten;
using Marten.Events.Aggregation;
using SharedContracts;

namespace eCommerce.Data
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public void Apply(Events.UserCreated @event)
        {
            Id = @event.Id;
            FirstName = @event.FirstName;
            LastName = @event.LastName;
        }

        public void Apply(Events.UserFirstNameUpdated @event)
        {
            FirstName = @event.FirstName;
        }
    }


    public class UserProjection : SingleStreamProjection<User, Guid>
    {

    }
}
