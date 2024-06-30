using SuccessPointCore.Application.Interfaces;
using SucessPointCore.Api.Domain.Helpers;
using SucessPointCore.Domain.Constants;
using SucessPointCore.Domain.Entities;
using SucessPointCore.Domain.Entities.Requests;
using SucessPointCore.Domain.Entities.Responses;
using SucessPointCore.Domain.Helpers;
using SucessPointCore.Infrastructure.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace SuccessPointCore.Application.Services
{
    public class UserService : IUserService
    {
        readonly IUserRepository _userRepository;
        readonly IEmailService _emailService;
        public UserService(IUserRepository userRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public int GetUserCount()
        {
            return _userRepository.GetUserCount();
        }


        public int CreateUser(CreateUserRequest userinfo)
        {
            var (encyptedPassword, passwordKey) = GetEncryptedPasswordAndPasswordKey(userinfo.Password);

            userinfo.PasswordKey = passwordKey;
            userinfo.EncryptedPassword = encyptedPassword;
            return _userRepository.AddUser(userinfo);
        }


        public bool UpdateUserInfo(CreateUserRequest userinfo)
        {
            return _userRepository.UpdateUserInfo(userinfo);
        }

        public bool UpdateUserPassword(UpdatePassword userinfo)
        {
            return _userRepository.UpdateUserPassword(userinfo);
        }


        public AuthenticatedUser CheckLoginCredentials(string username, string password)
        {

            string passwordKey = _userRepository.GetUserPasswordKey(username);
            if (string.IsNullOrWhiteSpace(passwordKey))
            {
                return null;
            }

            var encryptedPassword = ComputeSHA256Hash(password.Trim() + passwordKey + AppConfigHelper.PasswordEncyptionKey);

            return _userRepository.CheckCredentials(username, encryptedPassword);
        }

        public bool UpsertRefreshToken(UpsertRefreshToken tokenData)
        {
            return _userRepository.UpsertRefreshToken(tokenData);
        }

        public IEnumerable<EnrolledCoursesInfo> GetEnrolledCourses(int userID)
        {
            return _userRepository.GetEnrolledCourses(userID);
        }

        public (bool isValid, string message) ValidateLoginRequest(LoginUserRequest userinfo)
        {
            if (string.IsNullOrWhiteSpace(userinfo.GrantType) || userinfo.GrantType != "password")
            {
                return (false, "invalid grant type");
            }

            if (string.IsNullOrWhiteSpace(userinfo.UserName) || string.IsNullOrWhiteSpace(userinfo.Password))
            {
                return (false, "invalid credentials values");
            }

            return (true, string.Empty);
        }

        public bool ShouldCreateAdminUser(LoginUserRequest userinfo)
        {
            return userinfo.UserName.Trim() == "createadminuser" &&
                   userinfo.Password.Trim() == $"adm1n{DateTime.Now.ToString("ddMMyyyy")}pwd" &&
                   GetUserCount() == 0;
        }

        public void CreateAdminUser(string password)
        {
            CreateUser(new CreateUserRequest { UserName = "admin", Password = password, Active = true, UserType = 1 });
        }

        public (string Token, Guid RefreshToken) GenerateToken(AuthenticatedUser authenticatedUser)
        {
            return new JwtAuthManager(AppConfigHelper.JWTTokenEncryptionKey, AppConfigHelper.Issuer, AppConfigHelper.Audience).GenerateTokens(authenticatedUser);
        }

        public bool IsEmailAvailableForSignup(string userEmailId)
        {
            return _userRepository.IsEmailAvailableForSignup(userEmailId);
        }
        public string RegisterUserBySignup(SignupCredentials userdetails)
        {
            string vid = Guid.NewGuid().ToString();
            string tfc_code = new Random().Next(1111, 9999).ToString();
            string htmlContent = VerificationMailConstant.SignupEmailVerificationContent
                        .Replace("{{VID}}", vid)
                        .Replace("{{TFC}}", tfc_code);
            userdetails.VID = vid;
            userdetails.ExpiryTime = DateTime.UtcNow.AddMinutes(AppConfigHelper.VerificationExpiryMinute);
            userdetails.verificationType = SucessPointCore.Domain.Enums.EmailVerificationType.RegistrationEmail;

            // Generate encrypted password
            var passwordEncryptionResult = GetEncryptedPasswordAndPasswordKey(userdetails.Password);

            // Assign encypted password details
            userdetails.PasswordKey = passwordEncryptionResult.passwordKey;
            userdetails.EncryptedPassword = passwordEncryptionResult.encyptedPassword;

            // we should not save until email gone so that user can signup again if wanted to.

            var sendResponse = _emailService.SendSignupAccountVerificaitonLink(userdetails.EmailID, htmlContent, SucessPointCore.Domain.Enums.EmailVerificationType.RegistrationEmail);
            if (sendResponse)
            {
                _userRepository.SignupUser(userdetails);
            }


            return vid;

        }

        public StudentListResponse GetStudentList(int pageSize, int pageNo, string studentName)
        {
            return _userRepository.GetStudentList(pageSize, pageNo, studentName);
        }

        public IEnumerable<Standard> GetStandardList()
        {
            return _userRepository.GetStandardList();
        }

        public bool CreateStandard(string standardName)
        {
            return _userRepository.CreateStandard(standardName);
        }
        private (string encyptedPassword, string passwordKey) GetEncryptedPasswordAndPasswordKey(string plainPassword)
        {
            string passwordKey = new NumberGenerator().GenerateRandomText(10);
            return (ComputeSHA256Hash(plainPassword.Trim() + passwordKey + AppConfigHelper.PasswordEncyptionKey), passwordKey);
        }

        private string ComputeSHA256Hash(string input)
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
        private string GetUserPassworkdKey(string userName)
        {
            return _userRepository.GetUserPasswordKey(userName);
        }


    }
}
