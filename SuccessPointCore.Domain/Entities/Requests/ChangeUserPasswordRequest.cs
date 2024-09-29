using SuccessPointCore.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace SuccessPointCore.Domain.Entities.Requests
{
    public class ChangeUserPasswordRequest
    {
        /// <summary>
        /// Only Loggedin user can change its password.
        /// </summary>

        [StringLength(maximumLength: RequestValidaterConstant.PasswordMaxLength, MinimumLength = RequestValidaterConstant.PasswordMinLength, ErrorMessage = RequestValidaterConstant.InvalidPasswordLengthError)]
        public required string Password { get; set; }
    }
}
