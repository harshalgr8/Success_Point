using SuccessPointCore.Domain.Entities;
using SuccessPointCore.Domain.Entities.Requests;

namespace SuccessPointCore.Infrastructure.Interfaces
{
    public interface ICourseRepository
    {
        IEnumerable<Course> GetCourses();
        int UpsertCourse(int courseID, string courseName, bool isActive, int createdBy);

        IEnumerable<EnrolledCoursesInfo> GetEnrolledCourses(int userID);

        int UpsertVideoCourse(List<VideoCourse> courseVideos);
        List<VideoCourse> GetVideoCourse(int standardID, int CourseID);
        List<VideoCourse> GetUniqueVideoCourse();
        int RemoveCourseVideo(RemoveVideoCourseRequest removeVideoCourse);
    }
}
