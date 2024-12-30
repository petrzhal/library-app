using FluentValidation;
using Library.Application.DTOs.Book;

namespace Library.Application.Validations.Book
{
    public class BooksListRequestValidator : AbstractValidator<BookListRequest>
    {
        public BooksListRequestValidator()
        {
            RuleFor(x => x.PageIndex)
                .GreaterThan(0).WithMessage("Page index must be 0 or greater.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0.")
                .LessThanOrEqualTo(10).WithMessage("Page size must not exceed 10.");

            RuleFor(x => x.Genre).MaximumLength(50).WithMessage("Genre must not exceed 50 characters.");
            RuleFor(x => x.Title).MaximumLength(100).WithMessage("Title must not exceed 100 characters.");
        }
    }
}
