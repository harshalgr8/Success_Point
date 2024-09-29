using SuccessPointCore.Domain.Entities;
using SuccessPointCore.Domain.Entities.Requests;
using SuccessPointCore.Domain.Entities.Responses;

namespace SuccessPointCore.Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        int AddUser(CreateUserRequest userData);
        bool UpdateUserInfo(CreateUserRequest userData);

        bool UpdateUserPassword(UpdatePassword userData);

        int GetUserCount();

        string GetUserPasswordKey(string userName);

        AuthenticatedUser CheckCredentials(string userName, string encryptedPassword);

        bool UpsertRefreshToken(UpsertRefreshToken tokenData);
        bool IsEmailAvailableForSignup(string userEmailId);
        bool SignupUser(SignupCredentials userdetails);
        StudentListResponse GetStudentList(int pageSize, int pageNo, string studentName);

        bool UpdateStudentInfo(UpdateStudentRequest userData);

        bool ChangeStudentPassword(int studentID, string password,string PasswordKey);
        bool ChangeUserPassword(int userID, string password, string PasswordKey);

        bool RemoveStudent(int studentID);
    }
}
