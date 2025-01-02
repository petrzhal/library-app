using FluentValidation;
using Library.Application.DTOs.Images;

namespace Library.Application.Validations.Image
{
    public class GetImageRequestValidator : AbstractValidator<GetImageRequest>
    {
        public GetImageRequestValidator()
        {
            RuleFor(x => x.BookId).GreaterThan(0).WithMessage("BookId must be greater than 0.");
        }
    }
}
