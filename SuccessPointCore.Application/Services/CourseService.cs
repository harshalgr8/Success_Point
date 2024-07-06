using SuccessPointCore.Application.Interfaces;
using SucessPointCore.Domain.Entities;
using SucessPointCore.Infrastructure.Interfaces;

namespace SuccessPointCore.Application.Services
{
    public class CourseService : ICourseService
    {
        ICourseRepository _courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }
        public IEnumerable<Course> GetCourses()
        {
            return _courseRepository.GetCourses();
        }

        public int CreateCourse(string courseName, int createdBy)
        {
            return _courseRepository.AddCourse(courseName, createdBy);
        }

        public IEnumerable<EnrolledCoursesInfo> GetEnrolledCourses(int userID)
        {
            return _courseRepository.GetEnrolledCourses(userID);
        }

    }
}
