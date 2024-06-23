using SucessPointCore.Domain.Enums;

namespace SucessPointCore.Domain.Entities
{
    public class EmailVerification
    {
        public EmailVerificationType VerificationType { get; set; } = EmailVerificationType.RegistrationEmail;
    }
}
