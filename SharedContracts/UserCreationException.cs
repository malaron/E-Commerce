
namespace SharedContracts.Exceptions
{
    [Serializable]
    public class UserCreationException : Exception
    {
        public string[] Messages = [];

        public UserCreationException()
        {
        }

        public UserCreationException(string? message) : base(message)
        {
        }

        public UserCreationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public UserCreationException(string[] messages) : base(messages[0])
        {
            Messages = messages;
        }

        public UserCreationException(string[]? message, Exception? innerException) : base(message?[0], innerException)
        {
            Messages = message!;
        }
    }
}