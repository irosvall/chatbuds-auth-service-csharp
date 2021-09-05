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
			var payload = new JwtPayload
			{
				sub = accountId,
				name = accountUsername,
				exp = DateTimeOffset.Now.ToUnixTimeSeconds() + long.Parse(this._jwtConfig.Value.Token_Expire_Time)
			};

			var jwt = this._encoder
				.Encode(payload, this.GetFileText(this._jwtConfig.Value.Private_Key_Filepath));

			return jwt;
		}

		public Account AuthenticateJwt(IHeaderDictionary headers)
		{
			var authorization = headers["Authorization"][0]?.Split(" ");

			if (authorization?[0] != "Bearer")
			{
				throw new AuthenticationException();
			}

			var jwtPayloadJson = this._decoder
				.Decode(authorization[1], this.GetFileText(this._jwtConfig.Value.Public_Key_Filepath), true);

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
	}
}
