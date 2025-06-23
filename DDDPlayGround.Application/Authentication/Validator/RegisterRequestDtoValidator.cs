﻿using DDDPlayGround.Application.Authentication.Dtos;
using FluentValidation;

namespace DDDPlayGround.Application.Authentication.Validator
{
    public class RegisterRequestDtoValidator : AbstractValidator<RegisterRequestDto>
    {
        public RegisterRequestDtoValidator()
        {
            RuleFor(x => x.Username)
              .NotEmpty().WithMessage("Username is required.")
              .MinimumLength(3).WithMessage("Username must be at least 3 characters.");

            RuleFor(x => x.Email)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");
        }
    }
}

