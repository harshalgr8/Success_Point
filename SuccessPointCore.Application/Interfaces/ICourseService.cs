using SuccessPointCore.Domain.Entities;
using SuccessPointCore.Domain.Entities.Requests;

namespace SuccessPointCore.Application.Interfaces
{
    public interface ICourseService
    {
        IEnumerable<Course> GetCourses();

        int UpsertCourse(int courseId,string courseName, bool isActive, int createdBy);

        IEnumerable<EnrolledCoursesInfo> GetEnrolledCourses(int userID);

        int UpsertCourseVideo(List<VideoCourse> courseVideos);

        int RemoveCourseVideo(RemoveVideoCourseRequest removeVideoCourse);

        List<VideoCourse> GetVideoCourse(int standardID, int CourseID);

        List<VideoCourse> GetUniqueVideoCourse();

    }
}
