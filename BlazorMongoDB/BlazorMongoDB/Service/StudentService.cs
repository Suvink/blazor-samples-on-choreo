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
			var connectionString = configuration.GetValue<string>("MongoDB:ConnectionString") 
				?? Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING") 
				?? "mongodb://127.0.0.1:27017/";
			var databaseName = configuration.GetValue<string>("MongoDB:DatabaseName") 
				?? Environment.GetEnvironmentVariable("MONGODB_DATABASE_NAME") 
				?? "SchoolDB";
			var collectionName = configuration.GetValue<string>("MongoDB:CollectionName") 
				?? Environment.GetEnvironmentVariable("MONGODB_COLLECTION_NAME") 
				?? "Students";
				
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
