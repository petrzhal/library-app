using FluentValidation;
using Library.Application.DTOs.Authors;

namespace Library.Application.Validations.Author
{
    public class AuthorIdRequestValidator : AbstractValidator<AuthorIdRequest>
    {
        public AuthorIdRequestValidator()
        {
            RuleFor(x => x.AuthorId)
                .GreaterThan(0).WithMessage("Author ID must be greater than 0.");
        }
    }
}
