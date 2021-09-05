using System.Data;
using System.Security.Authentication;
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
		/// <exception cref="AuthenticationException">
		/// Throws if no account is found or password is wrong.
		/// </exception>
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
		/// <exception cref="DataException">
		/// Throws when no account was found to be deleted.
		/// </exception>
		Task DeleteAccount(Account account);
	}
}
