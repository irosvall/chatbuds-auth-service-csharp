using System.Security.Authentication;
using auth_service.Models;
using JWT.Exceptions;
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
		/// <exception cref="AuthenticationException">
        /// Thrown if bearer token is missing from the header.
        /// </exception>
        /// <exception cref="TokenExpiredException">
        /// Thrown if the token has expired.
        /// </exception>
        /// <exception cref="SignatureVerificationException">
		/// Thrown if the token has an invalid signature.
		/// </exception>
		Account AuthenticateJwt(IHeaderDictionary headers);
	}
}
