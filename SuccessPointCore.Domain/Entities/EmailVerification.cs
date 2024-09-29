using SuccessPointCore.Domain.Enums;

namespace SuccessPointCore.Domain.Entities
{
    public class EmailVerification
    {
        public EmailVerificationType VerificationType { get; set; } = EmailVerificationType.RegistrationEmail;
    }
}
