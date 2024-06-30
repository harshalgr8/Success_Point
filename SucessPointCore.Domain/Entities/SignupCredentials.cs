using SucessPointCore.Domain.Entities.Requests;
using SucessPointCore.Domain.Enums;

namespace SucessPointCore.Domain.Entities
{
    public class SignupCredentials : SignupUserByEmailRequest
    {
        public string EncryptedPassword { get; set; }

        public string PasswordKey { get; set; }

        public string VID { get; set; }
        public int TFC { get; set; }
        public DateTime ExpiryTime { get; set; }

        public EmailVerificationType verificationType { get; set; } = EmailVerificationType.RegistrationEmail;
    }
}
