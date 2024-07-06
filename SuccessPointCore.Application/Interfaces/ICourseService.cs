using SucessPointCore.Domain.Entities;

namespace SuccessPointCore.Application.Interfaces
{
    public interface ICourseService
    {
        IEnumerable<Course> GetCourses();

        int CreateCourse(string courseName, int createdBy);

        IEnumerable<EnrolledCoursesInfo> GetEnrolledCourses(int userID);

    }
}
