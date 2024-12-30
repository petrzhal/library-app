using FluentValidation;
using Library.Application.DTOs.Book;

namespace Library.Application.Validations.Book
{
    public class ReturnBookRequestValidator : AbstractValidator<ReturnBookRequest>
    {
        public ReturnBookRequestValidator()
        {
            RuleFor(x => x.BookId).GreaterThan(0).WithMessage("BookId must be greater than 0.");
        }
    }
}
