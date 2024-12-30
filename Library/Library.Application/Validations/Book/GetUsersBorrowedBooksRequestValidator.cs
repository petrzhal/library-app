using FluentValidation;
using Library.Application.DTOs.Book;

namespace Library.Application.Validations.Book
{
    public class GetUsersBorrowedBooksRequestValidator : AbstractValidator<GetUsersBorrowedBooksRequest>
    {
        public GetUsersBorrowedBooksRequestValidator()
        {
            RuleFor(x => x.PageIndex)
                .GreaterThan(0).WithMessage("Page index must be 0 or greater.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0.")
                .LessThanOrEqualTo(10).WithMessage("Page size must not exceed 10.");
        }
    }
}
