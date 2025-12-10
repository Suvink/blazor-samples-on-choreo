using BlazorMongoDB.Data;
using BlazorMongoDB.IService;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace BlazorMongoDB.Service
{
	public class StudentService : IStudentService
	{
		private MongoClient _mongoClient = null;
		private IMongoDatabase _database = null;
		private IMongoCollection<Student> _studentTable = null;
		
		public StudentService(IConfiguration configuration) 
		{
			// Try to get from configuration first (supports MongoDB__ConnectionString format)
			var connectionString = configuration["MongoDB:ConnectionString"];
			
			// If not found, try environment variable
			if (string.IsNullOrEmpty(connectionString))
			{
				connectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING");
			}
			
			// Fall back to default
			if (string.IsNullOrEmpty(connectionString))
			{
				connectionString = "mongodb://127.0.0.1:27017/";
			}
			
			var databaseName = configuration["MongoDB:DatabaseName"] 
				?? Environment.GetEnvironmentVariable("MONGODB_DATABASE_NAME") 
				?? "SchoolDB";
			var collectionName = configuration["MongoDB:CollectionName"] 
				?? Environment.GetEnvironmentVariable("MONGODB_COLLECTION_NAME") 
				?? "Students";
			
			// Log for debugging (remove in production)
			Console.WriteLine($"MongoDB Connection String: {connectionString?.Substring(0, Math.Min(20, connectionString.Length))}...");
			Console.WriteLine($"MongoDB Database: {databaseName}");
			Console.WriteLine($"MongoDB Collection: {collectionName}");
				
			_mongoClient = new MongoClient(connectionString);
			_database = _mongoClient.GetDatabase(databaseName);
			_studentTable = _database.GetCollection<Student>(collectionName);
		}

		public string Delete(string studentId) 
		{
			_studentTable.DeleteOne(x => x.Id == studentId);
			return "Deleted";
		}

		public Student GetStudent(string studentId)
		{
			return _studentTable.Find(x=>x.Id == studentId).FirstOrDefault();
		}

		public List<Student> GetStudents()
		{
			return _studentTable.Find(FilterDefinition<Student>.Empty).ToList();
		}

		public void SaveOrUpdate(Student student)
		{
			var studentObj =  _studentTable.Find(x=>x.Id == student.Id).FirstOrDefault();
			if (studentObj == null)
			{
				_studentTable.InsertOne(student);
			}
			else
			{
				_studentTable.ReplaceOne(x => x.Id == student.Id, student);
			}
		}
	}
}
