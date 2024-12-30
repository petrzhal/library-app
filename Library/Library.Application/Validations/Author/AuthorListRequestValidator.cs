﻿using FluentValidation;
using Library.Application.DTOs.Authors;

namespace Library.Application.Validations.Author
{
    public class AuthorListRequestValidator : AbstractValidator<AuthorListRequest>
    {
        public AuthorListRequestValidator()
        {
            RuleFor(x => x.PageIndex)
                .GreaterThan(0).WithMessage("Page index must be 0 or greater.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0.")
                .LessThanOrEqualTo(10).WithMessage("Page size must not exceed 10.");
        }
    }
}