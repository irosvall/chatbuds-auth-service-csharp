using System;
using System.IO;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text.Json;
using auth_service.Config;
using auth_service.Models;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace auth_service.Services.JwtService
{
	public class JwtService : IJwtService
	{
		private readonly IOptions<JwtConfig> _jwtConfig;
		private readonly IJwtEncoder _encoder;
		private readonly IJwtDecoder _decoder;

		public JwtService(IOptions<JwtConfig> jwtConfig)
		{
			this._jwtConfig = jwtConfig;

			var privateKey = this.GetRsaKey(this._jwtConfig.Value.Private_Key_Filepath);
			var publicKey = this.GetRsaKey(this._jwtConfig.Value.Public_Key_Filepath);

			var algorithm = new RS256Algorithm(publicKey, privateKey);
			var serializer = new JsonNetSerializer();
			var urlEncoder = new JwtBase64UrlEncoder();
			var provider = new UtcDateTimeProvider();
			var validator = new JwtValidator(serializer, provider);

			this._encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
			this._decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);
		}

		public string CreateJwt(string accountId, string accountUsername)
		{
			var payload = this.CreateJwtPayload(accountId, accountUsername);
			var privateKey = this.GetFileText(this._jwtConfig.Value.Private_Key_Filepath);

			var jwt = this._encoder
				.Encode(payload, privateKey);

			return jwt;
		}

		public Account AuthenticateJwt(IHeaderDictionary headers)
		{
			var token = this.GetBearerAuthorizationToken(headers);
			var publicKey = this.GetFileText(this._jwtConfig.Value.Public_Key_Filepath);

			var jwtPayloadJson = this._decoder
				.Decode(token, publicKey, true);

			var jwtPayload = JsonSerializer.Deserialize<JwtPayload>(jwtPayloadJson);

			return new Account
			{
				Id = jwtPayload?.sub,
				Username = jwtPayload?.name
			};
		}

		private RSA GetRsaKey(string filePath)
		{
			var keyText = this.GetFileText(filePath);
			var key = RSA.Create();
			key.ImportFromPem(keyText);
			return key;
		}

		private string GetFileText(string filePath)
		{
			return File.ReadAllText(filePath);
		}

		private long GetJwtPayloadExpireTime()
		{
			return DateTimeOffset.Now.ToUnixTimeSeconds() + long.Parse(this._jwtConfig.Value.Jwt_Expire_Time);
		}

		/// <exception cref="AuthenticationException">
		/// Thrown if bearer token is missing from the header.
		/// </exception>
		private string GetBearerAuthorizationToken(IHeaderDictionary headers)
		{
			var authorization = headers["Authorization"][0]?.Split(" ");

			if (authorization?[0] != "Bearer")
			{
				throw new AuthenticationException();
			}

			return authorization[1];
		}

		private JwtPayload CreateJwtPayload(string accountId, string accountUsername)
		{
			return new()
			{
				sub = accountId,
				name = accountUsername,
				exp = this.GetJwtPayloadExpireTime()
			};
		}
	}
}
