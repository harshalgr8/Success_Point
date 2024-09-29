using Dapper;
using MySqlConnector;
using Newtonsoft.Json;
using SuccessPointCore.Domain.Entities;
using SuccessPointCore.Domain.Entities.Requests;
using SuccessPointCore.Domain.Entities.Responses;
using SuccessPointCore.Domain.Helpers;
using SuccessPointCore.Infrastructure.Interfaces;
using System.Data;

namespace SuccessPointCore.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        public int AddUser(CreateUserRequest userData)
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

        public bool UpdateUserInfo(CreateUserRequest userData)
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

        public bool UpdateStudentInfo(UpdateStudentRequest userData)
        {
            using (IDbConnection conn = new MySqlConnection(AppConfigHelper.ConnectionString))
            {
                try
                {
                    conn.Open();

                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_UserID", userData.UserID);
                    parameters.Add("p_StudentID", userData.StudentID);
                    parameters.Add("p_DisplayName", userData.DisplayName);
                    parameters.Add("p_Username", userData.UserName);
                    parameters.Add("p_UserType", userData.UserType);
                    parameters.Add("p_Active", userData.Active);
                    parameters.Add("p_StandardID", userData.StandardID);
                    //parameters.Add("p_CourseID", userData.CourseID);
                    parameters.Add("p_CourseIDCsv", userData.CourseIDCsv);
                    return conn.Execute("sp_SP_Student_UpdateInfo", param: parameters) > 0;
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

                    return conn.QueryFirstOrDefault<AuthenticatedUser>("sp_SP_User_CheckCredentials", parameters);
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

        public bool UpsertRefreshToken(UpsertRefreshToken tokenData)
        {
            using (IDbConnection conn = new MySqlConnection(AppConfigHelper.ConnectionString))
            {
                try
                {
                    conn.Open();

                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_UserID", tokenData.UserID);
                    parameters.Add("p_RefreshToken", tokenData.RefreshToken);
                    parameters.Add("p_Token_Expiry_Minute", tokenData.RefreshTokenExpiryTime);

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

        public bool IsEmailAvailableForSignup(string userEmailId)
        {
            using (IDbConnection conn = new MySqlConnection(AppConfigHelper.ConnectionString))
            {
                try
                {
                    conn.Open();

                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_UserEmailId", userEmailId);

                    return !(conn.Execute("sp_SP_User_IsEmailAvailableForSignup", param: parameters) > 0);
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

        public bool SignupUser(SignupCredentials userdetails)
        {
            using (IDbConnection conn = new MySqlConnection(AppConfigHelper.ConnectionString))
            {
                try
                {
                    conn.Open();

                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_UserEmailId", userdetails.EmailID);
                    parameters.Add("p_EncryptedPassword", userdetails.EncryptedPassword);
                    parameters.Add("p_Passwordkey", userdetails.PasswordKey);

                    parameters.Add("p_VID", userdetails.VID);
                    parameters.Add("p_TFC", userdetails.TFC);
                    parameters.Add("p_ExpiryTime", userdetails.ExpiryTime);

                    return conn.Execute("sp_SP_User_SignupUser", param: parameters) > 0;
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

        public StudentListResponse GetStudentList(int pageSize, int pageNo, string studentName)
        {
            using (IDbConnection conn = new MySqlConnection(AppConfigHelper.ConnectionString))
            {
                try
                {
                    conn.Open();

                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_PageSize", pageSize);
                    parameters.Add("p_PageNo", pageNo);
                    parameters.Add("p_studentname", string.IsNullOrWhiteSpace(studentName) ? string.Empty : studentName.Trim());


                    var multi = conn.QueryMultiple("sp_SP_Student_GetStudentList", param: parameters);

                    var totalCount = multi.Read<int>().Single();
                    var students = multi.Read<Student>().ToList();


                    
                    return new StudentListResponse { TotalCount = totalCount, Students = students };
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

        public bool ChangeStudentPassword(int studentID, string encryptedPassword, string passwordKey)
        {
            using (IDbConnection conn = new MySqlConnection(AppConfigHelper.ConnectionString))
            {
                try
                {
                    conn.Open();

                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_StudentID", studentID);
                    parameters.Add("p_Password", encryptedPassword);
                    parameters.Add("p_Passwordkey", passwordKey);

                    return conn.Execute("sp_SP_Student_UpdatePassword", param: parameters) > 0;
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

        public bool ChangeUserPassword(int userID, string encryptedPassword, string passwordKey)
        {
            using (IDbConnection conn = new MySqlConnection(AppConfigHelper.ConnectionString))
            {
                try
                {
                    conn.Open();

                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_UserID", userID);
                    parameters.Add("p_Password", encryptedPassword);
                    parameters.Add("p_Passwordkey", passwordKey);

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

        public bool RemoveStudent(int studentID)
        {
            using (IDbConnection conn = new MySqlConnection(AppConfigHelper.ConnectionString))
            {
                try
                {
                    conn.Open();

                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_StudentId", studentID);

                    return conn.Execute("sp_sp_Student_Delete", param: parameters) > 0;
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
