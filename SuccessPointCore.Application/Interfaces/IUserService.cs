
using SucessPointCore.Domain.Entities;
using SucessPointCore.Domain.Entities.Requests;
using SucessPointCore.Domain.Entities.Responses;

namespace SuccessPointCore.Application.Interfaces
{
    public interface IUserService
    {
        int GetUserCount();

        int CreateUser(CreateUserRequest userinfo);

        //string ComputeSHA256Hash(string input);

        bool UpdateUserInfo(CreateUserRequest userinfo);

        bool UpdateUserPassword(UpdatePassword userinfo);

        //string GetUserPassworkdKey(string userName);

        AuthenticatedUser CheckLoginCredentials(string username, string password);

      
        bool UpsertRefreshToken(UpsertRefreshToken tokenData);

        

        (bool isValid, string message) ValidateLoginRequest(LoginUserRequest userinfo);

        bool ShouldCreateAdminUser(LoginUserRequest userinfo);

        void CreateAdminUser(string password);

        (string Token, Guid RefreshToken) GenerateToken(AuthenticatedUser authenticatedUser);

        bool IsEmailAvailableForSignup(string userEmailId);

        string RegisterUserBySignup(SignupCredentials userdetails);
        StudentListResponse GetStudentList(int pageSize, int pageNo, string studentName);
        

        
    }
}