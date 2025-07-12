namespace SharedContracts
{
    public class Events
    {
        public class UserCreated
        {
            public Guid Id { get; set; } = Guid.CreateVersion7();
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public record UserFirstNameUpdated(string FirstName);
        public record UserLastNameUpdated(string LastName);
        public record UserEmailUpdated(string Email);


    }
}
