using System.IO;
using System.Security.Cryptography;
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

		public JwtService(IOptions<JwtConfig> jwtConfig)
		{
			this._jwtConfig = jwtConfig;
		}

		public string CreateJwt(string accountId, string accountUsername)
		{
			var privateKey = this.GetRsaKey(this._jwtConfig.Value.Private_Key_Filepath);
			var publicKey = this.GetRsaKey(this._jwtConfig.Value.Public_Key_Filepath);

			var payload = new
			{
				sub = accountId,
				name = accountUsername,
				exp = int.Parse(this._jwtConfig.Value.Token_Expire_Time)
			};

			IJwtAlgorithm algorithm = new RS256Algorithm(publicKey, privateKey);
			IJsonSerializer serializer = new JsonNetSerializer();
			IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
			IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

			var jwt = encoder
				.Encode(payload, this.GetFileText(this._jwtConfig.Value.Private_Key_Filepath));

			return jwt;
		}

		public Account AuthenticateJwt(IHeaderDictionary headers)
		{
			throw new System.NotImplementedException();
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
