using FluentValidation;
using Library.Application.DTOs.Book;

namespace Library.Application.Validations.Book
{
    public class BorrowBookRequestValidator : AbstractValidator<BorrowBookRequest>
    {
        public BorrowBookRequestValidator()
        {
            RuleFor(x => x.BookId).GreaterThan(0).WithMessage("BookId must be greater than 0.");
        }
    }
}
