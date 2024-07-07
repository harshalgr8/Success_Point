using Dapper;
using MySqlConnector;
using SucessPointCore.Domain.Entities;
using SucessPointCore.Domain.Helpers;
using SucessPointCore.Infrastructure.Interfaces;
using System.Data;

namespace SucessPointCore.Infrastructure.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        public int UpsertCourse(int courseID, string courseName, int createdBy)
        {
            using (IDbConnection conn = new MySqlConnection(AppConfigHelper.ConnectionString))
            {
                try
                {
                    conn.Open();

                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_CourseName", courseName);
                    parameters.Add("p_CreatedBy", createdBy);

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
                        item.videoList = item.videoList.Select(x => new EnrolledCourse { VideoName = x.VideoName, videoUrl = "http://sp.premiersolution.in/api/videos?videoFileName=" + x.VideoName }).ToList();
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

    }
}
