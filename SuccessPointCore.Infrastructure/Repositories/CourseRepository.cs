using Dapper;
using MySqlConnector;
using Newtonsoft.Json;
using SuccessPointCore.Domain.Entities;
using SuccessPointCore.Domain.Entities.Requests;
using SuccessPointCore.Domain.Helpers;
using SuccessPointCore.Infrastructure.Interfaces;
using System.Data;
using System.Text.Json.Serialization;

namespace SuccessPointCore.Infrastructure.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        public int UpsertCourse(int courseID, string courseName, bool isActive, int createdBy)
        {
            using (IDbConnection conn = new MySqlConnection(AppConfigHelper.ConnectionString))
            {
                try
                {
                    conn.Open();

                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_CourseId", courseID);
                    parameters.Add("p_CourseName", courseName);
                    parameters.Add("p_IsActive", isActive);
                    parameters.Add("p_CreatedBy", createdBy);
                    parameters.Add("p_ChangedBy", createdBy);

                    var lastCourseID = conn.Execute("sp_SP_Course_Upsert", param: parameters);


                    return lastCourseID;

                }
                catch (Exception)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    throw;
                }
            }
        }

        public IEnumerable<Course> GetCourses()
        {

            using (IDbConnection conn = new MySqlConnection(AppConfigHelper.ConnectionString))
            {
                try
                {
                    conn.Open();

                    DynamicParameters parameters = new DynamicParameters();

                    var courseList = conn.Query<Course>("sp_SP_Course_GetCourseList", param: parameters);


                    return courseList;

                }
                catch (Exception)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    throw;
                }
            }
        }

        public IEnumerable<EnrolledCoursesInfo> GetEnrolledCourses(int userID)
        {

            using (IDbConnection conn = new MySqlConnection(AppConfigHelper.ConnectionString))
            {
                try
                {
                    conn.Open();

                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_UserID", userID);

                    var result = conn.Query<EnrolledCourse>("sp_SP_User_GetUserEnrolledCourses", param: parameters);

                    var enrolledCourseGroup = result.GroupBy(x => x.CourseName);
                    List<EnrolledCoursesInfo> courseList = new List<EnrolledCoursesInfo>();
                    foreach (var item in enrolledCourseGroup)
                    {
                        courseList.Add(new EnrolledCoursesInfo { CourseName = item.Key, videoList = item.Select(x => x).ToList() });
                    }

                    foreach (var item in courseList)
                    {
                        item.videoList = item.videoList.Select(x => new EnrolledCourse { CourseName = x.CourseName, StandardName = x.StandardName, VideoName = x.VideoName, VideoHeading = x.VideoHeading }).ToList();
                    }

                    return courseList;

                }
                catch (Exception)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    throw;
                }
            }
        }

        public int UpsertVideoCourse(List<VideoCourse> courseVideos)
        {
            using (IDbConnection conn = new MySqlConnection(AppConfigHelper.ConnectionString))
            {
                try
                {
                    conn.Open();

                    DynamicParameters parameters = new DynamicParameters();
                    var videoCourseList = JsonConvert.SerializeObject(courseVideos);
                    parameters.Add("p_course_list", videoCourseList);

                    var lastCourseID = conn.Execute("sp_sp_courseVideo_Upsert", param: parameters);

                    return lastCourseID;

                }
                catch (Exception)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    throw;
                }
            }
        }

        public List<VideoCourse> GetVideoCourse(int standardID, int CourseID)
        {
            using (IDbConnection conn = new MySqlConnection(AppConfigHelper.ConnectionString))
            {
                try
                {
                    conn.Open();

                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_courseId", CourseID);
                    parameters.Add("p_standardId", standardID);

                    return conn.Query<VideoCourse>("sp_sp_CourseVideo_GetCourse", param: parameters).ToList();


                }
                catch (Exception)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    throw;
                }
            }
        }

        public List<VideoCourse> GetUniqueVideoCourse()
        {
            using (IDbConnection conn = new MySqlConnection(AppConfigHelper.ConnectionString))
            {
                try
                {
                    conn.Open();

                    DynamicParameters parameters = new DynamicParameters();

                    return conn.Query<VideoCourse>("sp_sp_CourseVideo_GetStandardwiseUniqueCourse", param: parameters).ToList();

                }
                catch (Exception)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    throw;
                }
            }
        }

        public int RemoveCourseVideo(RemoveVideoCourseRequest removeVideoCourse) {
            using (IDbConnection conn = new MySqlConnection(AppConfigHelper.ConnectionString))
            {
                try
                {
                    conn.Open();

                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_CourseId", removeVideoCourse.CourseId);
                    parameters.Add("p_StandardId", removeVideoCourse.StandardId);


                    return conn.Execute("sp_sp_CourseVideo_remove_coursevideo", param: parameters);

                }
                catch (Exception)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    throw;
                }
            }
        }
    }
}
