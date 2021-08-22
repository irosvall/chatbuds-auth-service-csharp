using auth_service.Models;
using auth_service.Services.DbClient;
using MongoDB.Driver;

namespace auth_service.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly IMongoCollection<Account> _accounts;

        public AccountService(IDbClient dbClient)
        {
            _accounts = dbClient.GetAccountCollection();
        }
        
        public Account AuthenticateAccount(string email, string password)
        {
            throw new System.NotImplementedException();
        }

        public Account CreateAccount(Account account)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAccount(Account account)
        {
            throw new System.NotImplementedException();
        }
    }
}
