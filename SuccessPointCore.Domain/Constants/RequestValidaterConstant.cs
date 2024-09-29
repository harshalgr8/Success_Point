namespace SuccessPointCore.Domain.Constants
{
    public class RequestValidaterConstant
    {
        //Password
        public const int PasswordMinLength = 3;
        public const int PasswordMaxLength = 250;

        // UserName
        public const int UserNameMinLength = 3;
        public const int UserNameMaxLengh = 100;

        // StudentName
        public const int StudentNameMinLengh = 3;
        public const int StudentDisplayNameMaxLength = 50;

        //Users
        public const int UserMinimumID = 1;
        public const int UserMaximumID = int.MaxValue;

        public const string InvalidPasswordLengthError = "Invalid Password Length";
        public const string InvalidStudentIDError = "Invalid StudentID.";
    }
}
