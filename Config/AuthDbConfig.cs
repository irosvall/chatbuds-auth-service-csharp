namespace auth_service.Config
{
	/// <summary>
	/// The configuration of the authentication service's database.
	/// </summary>
	public class AuthDbConfig
	{
		// Automatically set from the launchSettings.
		public string Database_Name { get; set; }

		// Automatically set from the launchSettings.
		public string Account_Collection_Name { get; set; }

		// Automatically set from the user-secrets.
		public string Db_Connection_String { get; set; }
	}
}
