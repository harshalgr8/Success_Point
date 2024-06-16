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
    }
}
