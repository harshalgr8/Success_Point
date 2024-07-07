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

        public int UpsertCourse(int courseId, string courseName, int createdBy)
        {
            return _courseRepository.UpsertCourse(courseId,courseName, createdBy);
        }

        public IEnumerable<EnrolledCoursesInfo> GetEnrolledCourses(int userID)
        {
            return _courseRepository.GetEnrolledCourses(userID);
        }

    }
}
