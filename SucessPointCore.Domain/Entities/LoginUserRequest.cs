using System.ComponentModel.DataAnnotations;

namespace SucessPointCore.Domain.Entities
{
    public class LoginUserRequest : UserCredentials
    {
        [Required(ErrorMessage = "Valid GrantType value required.")]
        public string GrantType { get; set; }
    }
}
