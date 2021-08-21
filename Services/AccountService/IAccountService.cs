using auth_service.Models;

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
        Account CreateAccount(Account account);

        /// <summary>
        /// Deletes the account.
        /// </summary>
        void DeleteAccount(Account account);
    }
}