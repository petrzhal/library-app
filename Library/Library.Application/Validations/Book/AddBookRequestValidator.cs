using FluentValidation;
using Library.Application.DTOs.Book;

namespace Library.Application.Validations.Book
{
    public class AddBookRequestValidator : AbstractValidator<AddBookRequest>
    {
        public AddBookRequestValidator()
        {
            RuleFor(x => x.ISBN)
                .NotEmpty().WithMessage("ISBN is required.")
                .Length(13).WithMessage("ISBN must be exactly 13 characters long.")
                .Matches(@"^\d{13}$").WithMessage("ISBN must contain only digits.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(255).WithMessage("Title must not exceed 255 characters.");

            RuleFor(x => x.Genre)
                .NotEmpty().WithMessage("Genre is required.")
                .MaximumLength(100).WithMessage("Genre must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

            RuleFor(x => x.AuthorId)
                .GreaterThan(0).WithMessage("AuthorId must be a positive integer.");

            RuleFor(x => x.ImageData)
                .NotEmpty().WithMessage("Image data is required.")
                .Must(data => data.Length > 0).WithMessage("Image data cannot be empty.");

            RuleFor(x => x.ImageType)
                .NotEmpty().WithMessage("Image type is required.")
                .Must(type => type == "image/jpeg" || type == "image/png")
                .WithMessage("Image type must be either 'image/jpeg' or 'image/png'.");
        }
    }
}
