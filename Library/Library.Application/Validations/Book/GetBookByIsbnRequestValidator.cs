using FluentValidation;
using Library.Application.DTOs.Book;

namespace Library.Application.Validations.Book
{
    public class GetBookByIsbnRequestValidator : AbstractValidator<BookIsbnRequest>
    {
        public GetBookByIsbnRequestValidator()
        {
            RuleFor(x => x.Isbn)
                .NotEmpty().WithMessage("ISBN is required.")
                .Length(13).WithMessage("ISBN must be exactly 13 characters long.")
                .Matches(@"^\d{13}$").WithMessage("ISBN must contain only digits.");
        }
    }
}
