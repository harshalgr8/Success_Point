namespace SuccessPointCore.Domain.Constants
{
    public class MessageConstant
    {
        public const string InvalidCredentials = "Invalid Credentials Provided";
        public const string LoginSuccess = "Login Success";
        public const string InvalidGrantType = "Invalid Grant Type Provided";
        public const string DefaultUserCreated = "Default User Created";
        public const string InvalidDisplayName = "Invalid Display Name Provided";
        public const string InvalidPasswordLengh = "Invalid Password Length";
        public const string InvalidEmailID = "Valid Email ID required";
        public const string InvalidEmailIDForRegister = "Requested Email is already Registered.";

        //Verifications 
        public const string SignupVerificationEmailSent = "Account Signup Verification Email Sent";

        public const string InternalServerError = "Internal Error Occurred. Please Contact Us";

        //Course
        public const string ValidCourseNameRequired = "valid Course name required";

        //Standard

        public const string ValidStandardNameRequired = "Valid Standard name requried";
    }
}
