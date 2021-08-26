using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using auth_service.Models;
using auth_service.Services.DbClient;
using FluentValidation;
using MongoDB.Driver;

namespace auth_service.Services.AccountService
{
	public class AccountService : IAccountService
	{
		private readonly IMongoCollection<Account> _accounts;
		private readonly IValidator<Account> _validator;

        public AccountService(IDbClient dbClient, IValidator<Account> validator)
        {
            _accounts = dbClient.GetAccountCollection();
            _validator = validator;
        }
        
        public Account AuthenticateAccount(string email, string password)
        {
            throw new System.NotImplementedException();
        }

		public async Task<Account> RegisterAccount(Account account)
		{
			// Trims away white spaces from username and email.
			account.Email = account.Email.Trim();
			account.Username = account.Username.Trim();

			await this.ValidateAccount(account);

			// Hashes the password after successful validation.
			account.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);

			// Adds the account to the database.
			await this._accounts.InsertOneAsync(account);

			return account;
		}

		public void DeleteAccount(Account account)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Validates the Account.
		/// </summary>
		/// <exception cref="ValidationException">
		/// Throws when the validation of the account fails.
		/// </exception>
		private async Task ValidateAccount(Account account)
		{
			await this._validator.ValidateAndThrowAsync(account);
		}
	}
}
