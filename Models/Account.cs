using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace auth_service.Models
{
    public class Account
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        public string Email { get; set; }
        
        public string Username { get; set; }
        
        public string Password { get; set; }
    }
}
