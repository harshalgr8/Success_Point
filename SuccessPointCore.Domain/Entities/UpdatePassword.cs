using System.Text.Json.Serialization;

namespace SuccessPointCore.Domain.Entities
{
    public class UpdatePassword :  User
    {

        [JsonIgnore]
        public string EncryptedPassword { get; set; }

        [JsonIgnore]
        public string PasswordKey { get; set; }
    }
}
