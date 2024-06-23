using SucessPointCore.Domain.Constants;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SucessPointCore.Domain.Entities
{
    public class SignupUserByEmail
    {
        [Required]
        [EmailAddress(ErrorMessage = MessageConstant.InvalidEmailID)]
        public string EmailID { get; set; }

        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 6, ErrorMessage = MessageConstant.InvalidCredentials)]
        public string Password { get; set; }

        
    }
}
