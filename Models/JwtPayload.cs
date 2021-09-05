using System;

namespace auth_service.Models
{
	/// <summary>
	/// Defines the payload used for JWT handling.
	///
	/// Important that the properties are non-capitalized to work with the other services.
	/// </summary>
	public class JwtPayload
	{
		public string sub { get; set; }

		public string name { get; set; }

		public long exp { get; set; }
	}
}
