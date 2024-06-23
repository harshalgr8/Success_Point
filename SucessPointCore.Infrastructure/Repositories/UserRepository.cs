using Dapper;
using MySqlConnector;
using SucessPointCore.Domain.Entities;
using SucessPointCore.Domain.Helpers;
using SucessPointCore.Infrastructure.Interfaces;
using System.Data;
using System.Reflection.Metadata;

namespace SucessPointCore.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        public int AddUser(CreateUser userData)
        {
            using (IDbConnection conn = new MySqlConnection(AppConfigHelper.ConnectionString))
            {
                try
                {
                    conn.Open();

                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_Username", userData.UserName);
                    parameters.Add("p_Password", userData.EncryptedPassword);
                    parameters.Add("p_UserType", userData.UserType);
                    parameters.Add("p_Active", userData.Active);
                    parameters.Add("p_Passwordkey", userData.PasswordKey);
                    parameters.Add("p_DisplayName", userData.DisplayName);
                    return Convert.ToInt32(conn.ExecuteScalar("sp_SP_User_Insert", param: parameters));
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

        public bool UpdateUserInfo(CreateUser userData)
        {
            using (IDbConnection conn = new MySqlConnection(AppConfigHelper.ConnectionString))
            {
                try
                {
                    conn.Open();

                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_UserID", userData.UserID);
                    parameters.Add("p_DisplayName", userData.DisplayName);
                    parameters.Add("p_Mobile", userData.MobileNo);
                    parameters.Add("p_Username", userData.UserName);
                    parameters.Add("p_UserType", userData.UserType);
                    parameters.Add("p_Active", userData.Active);

                    return conn.Execute("sp_SP_User_UpdateInfo", param: parameters) > 0;
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

        public bool UpdateUserPassword(UpdatePassword userData)
        {
            using (IDbConnection conn = new MySqlConnection(AppConfigHelper.ConnectionString))
            {
                try
                {
                    conn.Open();

                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_UserID", userData.UserID);
                    parameters.Add("p_Password", userData.EncryptedPassword);
                    parameters.Add("p_Passwordkey", userData.PasswordKey);

                    return conn.Execute("sp_SP_User_UpdatePassword", param: parameters) > 0;
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

        public int GetUserCount()
        {
            using (IDbConnection conn = new MySqlConnection(AppConfigHelper.ConnectionString))
            {
                try
                {
                    conn.Open();

                    return Convert.ToInt32(conn.ExecuteScalar("sp_SP_User_GetCount"));
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

        public string GetUserPasswordKey(string userName)
        {
            using (IDbConnection conn = new MySqlConnection(AppConfigHelper.ConnectionString))
            {
                try
                {
                    conn.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_Username", userName);

                    return Convert.ToString(conn.ExecuteScalar("sp_SP_User_GetPasswordKeyByUserName", parameters));
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

        public AuthenticatedUser CheckCredentials(string userName, string encryptedPassword)
        {
            using (IDbConnection conn = new MySqlConnection(AppConfigHelper.ConnectionString))
            {
                try
                {
                    conn.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_Username", userName);
                    parameters.Add("p_EncryptedPassword", encryptedPassword);

                    return conn.QuerySingleOrDefault<AuthenticatedUser>("sp_SP_User_CheckCredentials", parameters);
                }
                catch (InvalidOperationException ex)
                {
                    return null;
                }
                catch (Exception)
                {
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

        public bool UpsertRefreshToken(UpsertRefreshToken tokenData)
        {
            using (IDbConnection conn = new MySqlConnection(AppConfigHelper.ConnectionString))
            {
                try
                {
                    conn.Open();

                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@p_UserID", tokenData.UserID);
                    parameters.Add("@p_RefreshToken", tokenData.RefreshToken);
                    parameters.Add("@p_Token_Expiry_Minute", tokenData.RefreshTokenExpiryTime);

                    return conn.Execute("sp_SP_RefreshToken_Upsert", param: parameters) > 0;
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
