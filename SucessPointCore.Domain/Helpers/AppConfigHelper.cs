namespace SucessPointCore.Domain.Helpers
{
    public class AppConfigHelper
    {
        public static string ConnectionString { get; set; }
        public static string PasswordEncyptionKey { get; set; }
        public static string JWTTokenEncryptionKey { get; set; }
        public static string Issuer { get; set; }
        public static string Audience { get; set; }

        public static int TokenExpiryMinute { get; set; } = 30;
        public static int RefreshTokenExpiryMinute { get; set; } = 60;

        public static int VerificationExpiryMinute { get; set; } = 30;

        public static string SMTPURL { get; set; }

        public static string SMTPPORT { get; set; }

        public static string SignupEmailCredentials { get; set; }
        public static string ForgetEmailCredentials { get; set; }
        public static string DeleteAccountEmailCredentials { get; set; }

    }
}
