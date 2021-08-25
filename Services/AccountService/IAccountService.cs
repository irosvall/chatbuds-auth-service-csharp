using auth_service.Models;
using FluentValidation;

namespace auth_service.Services.AccountService
{
    public interface IAccountService
    {
        /// <summary>
        /// Authenticates the account.
        /// </summary>
        Account AuthenticateAccount(string email, string password);
        
        /// <summary>
        /// Creates a new account.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Throws when the validation of the account fails.
        /// </exception>
        Account CreateAccount(Account account);

        /// <summary>
        /// Deletes the account.
        /// </summary>
        void DeleteAccount(Account account);
    }
}