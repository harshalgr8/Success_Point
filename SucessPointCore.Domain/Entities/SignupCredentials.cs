using SucessPointCore.Domain.Enums;

namespace SucessPointCore.Domain.Entities
{
    public class SignupCredentials : SignupUserByEmail
    {
        public string EncryptedPassword { get; set; }

        public string PasswordKey { get; set; }

        public string VID { get; set; }
        public int TFC { get; set; }
        public DateTime ExpiryTime { get; set; }

        public EmailVerificationType verificationType { get; set; } = EmailVerificationType.RegistrationEmail;
    }
}
