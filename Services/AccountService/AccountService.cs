using System.Data;
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
			this._accounts = dbClient.GetAccountCollection();
			this._validator = validator;
		}

		public async Task<Account> AuthenticateAccount(string email, string password)
		{
			var account = await this._accounts
				.Find(x => x.Email == email)
				.FirstOrDefaultAsync();

			if (account == null || this.IsWrongPassword(password, account.Password))
			{
				throw new AuthenticationException();
			}

			return account;
		}

		public async Task<Account> RegisterAccount(Account account)
		{
			this.TrimAccountNameAndEmail(account);
			await this.ValidateAccount(account);

			account.Password = this.CreateHashedPassword(account.Password);
			await this.AddAccountToDatabase(account);

			return account;
		}

		public async Task DeleteAccount(Account account)
		{
			var deleteResult = await this._accounts
				.DeleteOneAsync(x => x.Id == account.Id);

			if (deleteResult.DeletedCount == 0)
			{
				throw new DataException();
			}
		}

		/// <exception cref="ValidationException">
		/// Throws when the validation of the account fails.
		/// </exception>
		private async Task ValidateAccount(Account account)
		{
			await this._validator.ValidateAndThrowAsync(account);
		}

		private bool IsWrongPassword(string inputPassword, string correctPassword)
		{
			return !BCrypt.Net.BCrypt.Verify(inputPassword, correctPassword);
		}

		private void TrimAccountNameAndEmail(Account account)
		{
			account.Email = account.Email.Trim();
			account.Username = account.Username.Trim();
		}

		private string CreateHashedPassword(string password)
		{
			return BCrypt.Net.BCrypt.HashPassword(password);
		}

		private async Task AddAccountToDatabase(Account account)
		{
			await this._accounts.InsertOneAsync(account);
		}
	}
}
