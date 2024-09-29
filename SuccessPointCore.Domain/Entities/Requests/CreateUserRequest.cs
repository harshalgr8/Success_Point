using SuccessPointCore.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace SuccessPointCore.Domain.Entities.Requests
{
    public class CreateUserRequest : UpdatePassword
    {
        [StringLength(maximumLength: 50, MinimumLength = 1, ErrorMessage = MessageConstant.InvalidDisplayName)]
        public string DisplayName { get; set; }
        public int MobileNo { get; set; }
        public bool Active { get; set; }
    }
}
