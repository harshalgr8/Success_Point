using SucessPointCore.Domain.Entities;
using SucessPointCore.Domain.Entities.Requests;
using SucessPointCore.Domain.Entities.Responses;

namespace SucessPointCore.Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        int AddUser(CreateUserRequest userData);
        bool UpdateUserInfo(CreateUserRequest userData);

        bool UpdateUserPassword(UpdatePassword userData);

        int GetUserCount();

        string GetUserPasswordKey(string userName);

        AuthenticatedUser CheckCredentials(string userName, string encryptedPassword);

        IEnumerable<EnrolledCoursesInfo> GetEnrolledCourses(int userID);
        bool UpsertRefreshToken(UpsertRefreshToken tokenData);
        bool IsEmailAvailableForSignup(string userEmailId);
        bool SignupUser(SignupCredentials userdetails);
        StudentListResponse GetStudentList(int pageSize, int pageNo, string studentName);
        IEnumerable<Standard> GetStandardList();
        bool CreateStandard(string standardName);
    }
}
