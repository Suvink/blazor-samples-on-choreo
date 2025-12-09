using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BlazorMongoDB.Data
{
	public class Student
	{
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; } = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
		public string Name { get; set; } = "";
		public string Roll { get; set; } = "";
		public int Age { get; set; } = 0;
	}
}
