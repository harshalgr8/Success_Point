using SuccessPointCore.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace SuccessPointCore.Domain.Entities.Requests
{
    public class ChangeStudentPasswordRequest
    {
        /// <summary>
        /// If logged in user is admin then we use StudentID.
        /// If logged in user is student then we use authorized userid.
        /// </summary>
        [Required]
        [Range(minimum: RequestValidaterConstant.UserMinimumID , maximum:RequestValidaterConstant.UserMaximumID, ErrorMessage = RequestValidaterConstant. InvalidStudentIDError)]
        public int StudentID { get; set; }

        [StringLength(maximumLength:RequestValidaterConstant.PasswordMaxLength, MinimumLength =RequestValidaterConstant.PasswordMinLength, ErrorMessage =RequestValidaterConstant.InvalidPasswordLengthError)]
        public required string Password { get; set; }
    }
}
