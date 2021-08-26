using auth_service.Models;
using Microsoft.AspNetCore.Http;

namespace auth_service.Services.JwtService
{
	public interface IJwtService
	{
		/// <summary>
		/// Creates a JWT with RS256 algorithm and with a payload containing the account's ID and username.
		/// </summary>
		string CreateJwt(string accountId, string accountUsername);

		/// <summary>
		/// Authenticates the http request by validating its JWT.
		/// </summary>
		Account AuthenticateJwt(IHeaderDictionary headers);
	}
}
