﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace auth_service.Models
{
	public class Account
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		[BsonElement("email")]
		public string Email { get; set; }

		[BsonElement("username")]
		public string Username { get; set; }

		[BsonElement("password")]
		public string Password { get; set; }
	}
}
