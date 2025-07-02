using DDDPlayGround.Application.Authentication.Dtos;
using FluentValidation;

namespace DDDPlayGround.Application.Authentication.Validator
{
    public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
    {
        public LoginRequestDtoValidator()
        {
            RuleFor(x => x.Username)
               .NotEmpty().WithMessage("Username is required.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .Must(content => !ContainsScripts(content));
        }

        private bool ContainsScripts(string input)
        {
            return input.Contains("<script>", StringComparison.OrdinalIgnoreCase)
                || input.Contains("javascript:", StringComparison.OrdinalIgnoreCase);
        }
    }
}
