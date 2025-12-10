using BlazorMongoDB.Data;
using BlazorMongoDB.IService;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

namespace BlazorMongoDB.Service
{
	public class StudentService : IStudentService
	{
		private MongoClient _mongoClient = null;
		private IMongoDatabase _database = null;
		private IMongoCollection<Student> _studentTable = null;
		
		public StudentService(IConfiguration configuration) 
		{
			// Get connection string from configuration or environment variable
			var connectionString = configuration["MongoDB:ConnectionString"];
			if (string.IsNullOrEmpty(connectionString))
			{
				connectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING");
			}
			if (string.IsNullOrEmpty(connectionString))
			{
				throw new InvalidOperationException("MongoDB connection string not configured. Set MongoDB__ConnectionString or MONGODB_CONNECTION_STRING.");
			}
			
			var databaseName = configuration["MongoDB:DatabaseName"] 
				?? Environment.GetEnvironmentVariable("MONGODB_DATABASE_NAME") 
				?? "SchoolDB";
			var collectionName = configuration["MongoDB:CollectionName"] 
				?? Environment.GetEnvironmentVariable("MONGODB_COLLECTION_NAME") 
				?? "Students";
			
			// Configure MongoDB client settings with SSL/TLS settings for Atlas
			var settings = MongoClientSettings.FromConnectionString(connectionString);
			settings.SslSettings = new SslSettings
			{
				EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12
			};
			settings.ServerApi = new ServerApi(ServerApiVersion.V1);
			
			_mongoClient = new MongoClient(settings);
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
