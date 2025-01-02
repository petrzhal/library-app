namespace Library.Application.Common.Exceptions
{
    public class BookAlreadyBorrowedException : Exception
    {
        public BookAlreadyBorrowedException(string message) : base(message) { }
    }
}
