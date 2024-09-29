using System.ComponentModel.DataAnnotations;

namespace SuccessPointCore.Domain.Entities.Requests
{
    public class LoginUserRequest : UserCredentials
    {
        [Required(ErrorMessage = "Valid GrantType value required.")]
        public string GrantType { get; set; }
    }
}
