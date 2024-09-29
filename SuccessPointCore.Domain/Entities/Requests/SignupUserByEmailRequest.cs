using SuccessPointCore.Domain.Constants;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SuccessPointCore.Domain.Entities.Requests
{
    public class SignupUserByEmailRequest
    {
        [Required]
        [EmailAddress(ErrorMessage = MessageConstant.InvalidEmailID)]
        public string EmailID { get; set; }

        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 6, ErrorMessage = MessageConstant.InvalidCredentials)]
        public string Password { get; set; }


    }
}
