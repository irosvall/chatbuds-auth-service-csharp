using System.Threading.Tasks;
using auth_service.Models;
using FluentValidation;

namespace auth_service.Services.AccountService
{
	public interface IAccountService
	{
		/// <summary>
		/// Authenticates the account.
		/// </summary>
		Task<Account> AuthenticateAccount(string email, string password);

		/// <summary>
		/// Creates a new account and hashes the password.
		/// </summary>
		/// <exception cref="ValidationException">
		/// Throws when the validation of the account fails.
		/// </exception>
		Task<Account> RegisterAccount(Account account);

		/// <summary>
		/// Deletes the account.
		/// </summary>
		void DeleteAccount(Account account);
	}
}
