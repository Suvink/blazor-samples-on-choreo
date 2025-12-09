using BlazorMongoDB.Data;

namespace BlazorMongoDB.IService
{
	public interface IStudentService
	{
		void SaveOrUpdate(Student student);

		Student GetStudent(string studentId);

		List<Student> GetStudents();

		string Delete(string studentId);
	}
}
