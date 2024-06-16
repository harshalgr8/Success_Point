using Dapper;
using MySqlConnector;
using SuccessPointCore.Application.Interfaces;
using SucessPointCore.Domain.Entities;
using SucessPointCore.Domain.Helpers;
using SucessPointCore.Infrastructure.Interfaces;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace SuccessPointCore.Application.Services
{
    public class UserService : IUserService
    {
        IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;

        }

        public int GetUserCount()
        {
            return _userRepository.GetUserCount();
        }

        public int CreateUser(CreateUser userinfo)
        {
            string passwordKey = new NumberGenerator().GenerateRandomText(10);

            userinfo.PasswordKey = passwordKey;
            userinfo.EncryptedPassword = ComputeSHA256Hash(userinfo.Password.Trim() + passwordKey + AppConfigHelper.PasswordEncyptionKey);
            return _userRepository.AddUser(userinfo);
        }

        public string ComputeSHA256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        public bool UpdateUserInfo(CreateUser userinfo)
        {
            return _userRepository.UpdateUserInfo(userinfo);
        }

        public bool UpdateUserPassword(UpdatePassword userinfo)
        {
            return _userRepository.UpdateUserPassword(userinfo);
        }

        private string GetUserPassworkdKey(string userName)
        {

            return _userRepository.GetUserPasswordKey(userName);
        }
        public AuthenticatedUser CheckLoginCredentials(string username, string password)
        {

            string passwordKey = _userRepository.GetUserPasswordKey(username);
            if (string.IsNullOrWhiteSpace(passwordKey))
            {
                return new AuthenticatedUser();
            }

            var encryptedPassword = ComputeSHA256Hash(password.Trim() + passwordKey + AppConfigHelper.PasswordEncyptionKey);

            return _userRepository.CheckCredentials(username, encryptedPassword);
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

        public IEnumerable<EnrolledCoursesInfo> GetEnrolledCourses(int userID)
        {
            return _userRepository.GetEnrolledCourses(userID);
        }

    }
}