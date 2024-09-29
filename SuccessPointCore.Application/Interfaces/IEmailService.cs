using SuccessPointCore.Domain.Entities;
using SuccessPointCore.Domain.Enums;

namespace SuccessPointCore.Application.Interfaces
{
    public interface IEmailService
    {
        bool SendSignupAccountVerificaitonLink(string userEmail, string emailContent, EmailVerificationType emailVerificationType);
        bool SendForgetPasswordVerificaitonLink(string userEmail, string emailContent, EmailVerificationType emailVerificationType);
        bool SendDeleteAccountVerificaitonLink(string userEmail, string emailContent, EmailVerificationType emailVerificationType);
    }
}
