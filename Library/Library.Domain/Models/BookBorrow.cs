namespace Library.Domain.Models
{
    public class BookBorrow : BaseModel
    {
        public int BookId { get; set; }
        public int UserId { get; set; }
        public Book? Book { get; set; }
        public User? User { get; set; }
    }
}
