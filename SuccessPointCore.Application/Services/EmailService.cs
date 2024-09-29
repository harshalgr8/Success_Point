using SuccessPointCore.Application.Interfaces;
using SuccessPointCore.Domain.Entities;
using SuccessPointCore.Domain.Enums;
using SuccessPointCore.Domain.Helpers;
using System.Net;
using System.Net.Mail;

namespace SuccessPointCore.Application.Services
{
    public class EmailService : IEmailService
    {
        IErrorLogService ErrorLogService { get; set; }
        public EmailService(IErrorLogService errorLogService)
        {
            ErrorLogService = errorLogService;
        }
        public bool SendSignupAccountVerificaitonLink(string userEmail, string emailContent, EmailVerificationType emailVerificationType)
        {
            return SendEmail(userEmail, emailContent, "Verify Your Email To Delete Registered Account with Success Point", emailVerificationType);
        }
       
        public bool SendForgetPasswordVerificaitonLink(string userEmail, string emailContent, EmailVerificationType emailVerificationType)
        {
            return SendEmail(userEmail, emailContent, "Password Recovery for Account Registered with Success Point", emailVerificationType);
        }
        public bool SendDeleteAccountVerificaitonLink(string userEmail, string emailContent, EmailVerificationType emailVerificationType)
        {
            return SendEmail(userEmail, emailContent, "Verify Your Email To Delete Registered Account with Success Point", emailVerificationType);
        }


        private bool SendEmail(string userEmailid, string emailContent, string subject, EmailVerificationType emailType)
        {
            string email_UserName = string.Empty;
            string email_Password = string.Empty;

            switch (emailType)
            {
                case EmailVerificationType.RegistrationEmail:
                    var Signup_credentials = AppConfigHelper.SignupEmailCredentials.Split(',');
                    email_UserName = Signup_credentials[0];
                    email_Password = Signup_credentials[1];
                    break;
                case EmailVerificationType.ForgetPasswordEmail:
                    var forget_credentials = AppConfigHelper.ForgetEmailCredentials.Split(',');
                    email_UserName = forget_credentials[0];
                    email_Password = forget_credentials[1];
                    break;
                case EmailVerificationType.AccountDeleteEmail:
                    var deleteaccount_credentials = AppConfigHelper.DeleteAccountEmailCredentials.Split(',');
                    email_UserName = deleteaccount_credentials[0];
                    email_Password = deleteaccount_credentials[1];
                    break;

            }

            // Configure the SMTP client
            var smtpClient = new SmtpClient(AppConfigHelper.SMTPURL)
            {
                Port = int.Parse(AppConfigHelper.SMTPPORT), // Use the appropriate port for your webmail provider

                Credentials = new NetworkCredential(email_UserName, email_Password),

                EnableSsl = true, // Use SSL if required by your webmail provider
            };

            // Create the email message
            var message = new MailMessage
            {
                From = new MailAddress(email_UserName),
                Subject = subject,
                Body = emailContent,
                IsBodyHtml = true,
            };
            message.To.Add(userEmailid);

            try
            {
                // Send the email
                smtpClient.Send(message);
                Console.WriteLine("Email sent successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
                ErrorLogService.AddError(new CreateErrorLog { ErrorMesage = ex.Message, StackTrace = ex.StackTrace, UserID = null });
                return false;
            }
        }
    }
}
