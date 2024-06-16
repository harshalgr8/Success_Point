namespace SucessPointCore.Domain.Entities
{
    public class SecurityKeys
    {
        public string PasswordEncryptionKey { get; set; }
        public string JWTTokenEncryptionKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
