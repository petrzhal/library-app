using FluentValidation;
using Library.Application.DTOs.Authors;

namespace Library.Application.Validations.Author
{
    public class DeleteAuthorRequestValidator : AbstractValidator<DeleteAuthorRequest>
    {
        public DeleteAuthorRequestValidator()
        {
            RuleFor(x => x.AuthorId)
                .GreaterThan(0).WithMessage("Author ID must be greater than 0.");
        }
    }
}
