namespace auth_service.Config
{
	/// <summary>
	/// The configuration of the authentication service's database.
	/// </summary>
	public class AuthDbConfig
	{
		// The database name is set automatically from the launchSettings.
		public string Database_Name { get; set; }

		// The account collection name is set automatically from the launchSettings.
		public string Account_Collection_Name { get; set; }

		// The database connection string is set automatically from the user-secrets.
		public string Db_Connection_String { get; set; }
	}
}
