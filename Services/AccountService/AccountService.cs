using System;
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

        public Account CreateAccount(Account account)
        {
            ValidateAccount(account);
            account.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
            _accounts.InsertOneAsync(account).Wait(); 
            return account;
        }

        public void DeleteAccount(Account account)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Validates the Account.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Throws when the validation of the account fails.
        /// </exception>
        private void ValidateAccount(Account account)
        {
            _validator.ValidateAndThrow(account);
        }
    }
}
