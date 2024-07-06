using SucessPointCore.Domain.Entities;

namespace SucessPointCore.Infrastructure.Interfaces
{
    public interface ICourseRepository
    {
        IEnumerable<Course> GetCourses();
        int AddCourse(string courseName, int createdBy);

        IEnumerable<EnrolledCoursesInfo> GetEnrolledCourses(int userID);
    }
}
