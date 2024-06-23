
using SucessPointCore.Domain.Entities;

namespace SuccessPointCore.Application.Interfaces
{
    public interface IUserService
    {
        int GetUserCount();

        int CreateUser(CreateUser userinfo);

        string ComputeSHA256Hash(string input);

        bool UpdateUserInfo(CreateUser userinfo);

        bool UpdateUserPassword(UpdatePassword userinfo);

        //string GetUserPassworkdKey(string userName);

        AuthenticatedUser CheckLoginCredentials(string username, string password);

      
        bool UpsertRefreshToken(UpsertRefreshToken tokenData);

        IEnumerable<EnrolledCoursesInfo> GetEnrolledCourses(int userID);

        (bool isValid, string message) ValidateLoginRequest(LoginUserRequest userinfo);

        bool ShouldCreateAdminUser(LoginUserRequest userinfo);

        void CreateAdminUser(string password);

        (string Token, Guid RefreshToken) GenerateToken(AuthenticatedUser authenticatedUser);
    }
}