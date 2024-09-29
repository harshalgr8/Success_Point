using SuccessPointCore.Application.Interfaces;
using SuccessPointCore.Domain.Entities;
using SuccessPointCore.Domain.Entities.Requests;
using SuccessPointCore.Infrastructure.Interfaces;

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

        public int UpsertCourse(int courseId, string courseName, bool isActive, int createdBy)
        {
            return _courseRepository.UpsertCourse(courseId, courseName, isActive, createdBy);
        }

        public IEnumerable<EnrolledCoursesInfo> GetEnrolledCourses(int userID)
        {
            return _courseRepository.GetEnrolledCourses(userID);
        }

        public int UpsertCourseVideo(List<VideoCourse> courseVideos)
        {
            return _courseRepository.UpsertVideoCourse(courseVideos);
        }

        public int RemoveCourseVideo(RemoveVideoCourseRequest removeVideoCourse) {
            return _courseRepository.RemoveCourseVideo(removeVideoCourse);
        }
        public List<VideoCourse> GetVideoCourse(int standardID, int CourseID)
        {
            return _courseRepository.GetVideoCourse(standardID, CourseID);
        }

        public List<VideoCourse> GetUniqueVideoCourse()
        {
            return _courseRepository.GetUniqueVideoCourse();
        }
    }
}
