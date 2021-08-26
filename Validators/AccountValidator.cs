using System.Text.RegularExpressions;
using System.Threading.Tasks;
using auth_service.Models;
using auth_service.Services.DbClient;
using FluentValidation;
using EmailValidation;
using MongoDB.Driver;

namespace auth_service.Validators
{
	/// <summary>
	/// Validates an account.
	/// </summary>
	public class AccountValidator : AbstractValidator<Account>
	{
		private readonly IMongoCollection<Account> _accounts;

		public AccountValidator(IDbClient dbClient)
		{
			this._accounts = dbClient.GetAccountCollection();

			this.RuleFor(account => account.Email)
				.Cascade(CascadeMode.Stop)
				.Must(this.IsValidEmail)
				.WithMessage("The email is not a valid email address.")
				.MustAsync(async (email, _) => await this.IsUnique("email", email))
				.WithMessage("The email is already in use.");

			this.RuleFor(account => account.Username)
				.Cascade(CascadeMode.Stop)
				.Length(2, 24)
				.WithMessage("The username must be between 2-24 characters.")
				.Must(this.IsAlphanumeric)
				.WithMessage("The username is only allowed to contain numbers and letters (a-z).")
				.MustAsync(async (username, _) => await this.IsUnique("username", username))
				.WithMessage("The username is already in use.");

			this.RuleFor(account => account.Password)
				.Length(10, 1000)
				.WithMessage("The password must be between 10-1000 characters.");
		}

		private bool IsAlphanumeric(string name)
		{
			var regex = new Regex(@"^[a-zA-Z-0-9]*$");
			return regex.IsMatch(name);
		}

		private bool IsValidEmail(string email)
		{
			return EmailValidator.Validate(email);
		}

		/// <summary>
		/// Ensures that the value of the field is not already in use in the database.
		/// </summary>
		private async Task<bool> IsUnique(string fieldName, string value)
		{
			var result = await this._accounts
				.Find(Builders<Account>.Filter.Eq(fieldName, value))
				.ToListAsync();

			return result.Count == 0;
		}
	}
}
