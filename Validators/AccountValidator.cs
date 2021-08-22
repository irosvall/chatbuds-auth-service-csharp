using System.Text.RegularExpressions;
using auth_service.Models;
using FluentValidation;
using EmailValidation;

namespace auth_service.Validators
{
    /// <summary>
    /// Validates an account.
    /// </summary>
    public class AccountValidator : AbstractValidator<Account>
    {
        public AccountValidator()
        {
            RuleFor(account => account.Email)
                .Must(BeValidEmail)
                .WithMessage("The email is not a valid email address.");
            
            RuleFor(account => account.Username)
                .Cascade(CascadeMode.Stop)
                .Length(2, 24)
                .WithMessage("The username must be between 2-24 characters.")
                .Must(BeAlphanumeric)
                .WithMessage("The username is only allowed to contain numbers and letters (a-z).");

            RuleFor(account => account.Password)
                .Length(10, 1000)
                .WithMessage("The password must be between 10-1000 characters.");
        }

        private bool BeAlphanumeric(string name)
        {
            var regex = new Regex(@"^[a-zA-Z-0-9]*$");
            return regex.IsMatch(name);
        }

        private bool BeValidEmail(string email)
        {
            return EmailValidator.Validate(email);
        }
    }
}
