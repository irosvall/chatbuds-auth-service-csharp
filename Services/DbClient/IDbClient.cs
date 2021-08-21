using auth_service.Models;
using MongoDB.Driver;

namespace auth_service.Services.DbClient
{
    /// <summary>
    /// Interface for the database client.
    /// </summary>
    public interface IDbClient
    {
        /// <summary>
        /// Returns the accounts collection of the database.
        /// </summary>
        IMongoCollection<Account> GetAccountCollection();
    }
}
