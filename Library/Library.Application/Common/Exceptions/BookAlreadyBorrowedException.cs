namespace Library.Application.Common.Exceptions
{
    public class BookAlreadyBorrowedException(string message = "") : Exception(message) { }
}
