using auth_service.Config;
using auth_service.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace auth_service.Services.DbClient
{
	/// <summary>
	/// Database client for communication with the authentication service's database.
	/// </summary>
	public class DbClient : IDbClient
	{
		private readonly IMongoCollection<Account> _accounts;

		/// <summary>
		/// Initializes the database connection.
		/// </summary>
		public DbClient(IOptions<AuthDbConfig> authDbConfig)
		{
			var mongoClient = new MongoClient(authDbConfig.Value.Db_Connection_String);
			var database = mongoClient.GetDatabase(authDbConfig.Value.Database_Name);
			this._accounts = database.GetCollection<Account>(authDbConfig.Value.Account_Collection_Name);
		}

		public IMongoCollection<Account> GetAccountCollection() => this._accounts;
	}
}
