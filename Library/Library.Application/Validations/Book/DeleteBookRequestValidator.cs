using FluentValidation;
using Library.Application.DTOs.Book;

namespace Library.Application.Validations.Book
{
    public class DeleteBookRequestValidator : AbstractValidator<DeleteBookRequest>
    {
        public DeleteBookRequestValidator() 
        {
            RuleFor(x => x.BookId)
                .NotEmpty().WithMessage("Id is required.")
                .GreaterThanOrEqualTo(0).WithMessage("Id must be greater than zero.");
        }
    }
}
