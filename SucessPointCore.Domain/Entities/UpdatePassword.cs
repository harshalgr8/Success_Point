using System.Text.Json.Serialization;

namespace SucessPointCore.Domain.Entities
{
    public class UpdatePassword : User
    {

        [JsonIgnore]
        public string EncryptedPassword { get; set; }

        [JsonIgnore]
        public string PasswordKey { get; set; }
    }
}
