using Library.Application.DTOs.Authors;
using MediatR;

namespace Library.Application.DTOs.Book
{
    public class BookDto : IRequest<Unit>
    {
        public int Id { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public AuthorDto? Author { get; set; }
        public DateTime? BorrowedAt { get; set; }
        public DateTime? ReturnBy { get; set; }
        public string ImageId { get; set; }
    }
}
