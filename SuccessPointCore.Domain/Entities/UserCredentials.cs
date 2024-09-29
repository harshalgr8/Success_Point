using System.ComponentModel.DataAnnotations;

namespace SuccessPointCore.Domain.Entities
{
    public class UserCredentials
    {
        [Required(ErrorMessage = "Username Required")]
        public string UserName { get; set; }

        [StringLength(maximumLength: 25, MinimumLength = 3, ErrorMessage = "Valid password Required. Max Length : 25; Min Length :3 ")]
        public string Password { get; set; }
    }
}
