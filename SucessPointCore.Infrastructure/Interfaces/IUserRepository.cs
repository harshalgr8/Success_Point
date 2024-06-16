using SucessPointCore.Domain.Entities;

namespace SucessPointCore.Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        int AddUser(CreateUser userData);
        bool UpdateUserInfo(CreateUser userData);

        bool UpdateUserPassword(UpdatePassword userData);

        int GetUserCount();

        string GetUserPasswordKey(string userName);

        AuthenticatedUser CheckCredentials(string userName, string encryptedPassword);

        IEnumerable<EnrolledCoursesInfo> GetEnrolledCourses(int userID);
    }
}
