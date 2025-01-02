namespace Library.Application.Common.Exceptions
{
    public class EntityNotFoundException(string message = "") : Exception(message) { }
}
