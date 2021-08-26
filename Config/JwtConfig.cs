namespace auth_service.Config
{
	/// <summary>
	/// The configuration of the authentication service's JWT handling.
	/// </summary>
	public class JwtConfig
	{
		// Automatically from the user-secrets.
		public string Private_Key_Filepath { get; set; }

		// Automatically from the user-secrets.
		public string Public_Key_Filepath { get; set; }

		// Automatically from the user-secrets.
		public string Token_Expire_Time { get; set; }
	}
}
