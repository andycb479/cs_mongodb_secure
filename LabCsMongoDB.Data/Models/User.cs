using LabCsMongoDB.Data.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace LabCsMongoDB.Data.Models
{
     [BsonCollection("users")]
     public class User: Document
     {

          [BsonElement("name")]
          public string Name { get; set; }

          [BsonElement("password")]
          public string Password { get; set; }

          [BsonElement("cardNumber")]
          public string CardNumber { get; set; }

          [BsonElement("email")]
          public string Email { get; set; }

     }
}