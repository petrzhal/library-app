namespace Library.Application.Common.Exceptions
{
    public class UserAlreadyExistsException(string message = "") : Exception(message) { }
}
