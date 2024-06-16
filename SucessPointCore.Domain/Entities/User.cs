using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SucessPointCore.Domain.Entities
{
    public class User : UserCredentials
    {
       

        [JsonIgnore]
        public int UserID { get; set; }

        [Range(maximum: 3, minimum: 1, ErrorMessage = "Valid UserType Required")]
        public int UserType { get; set; }
    }
}
