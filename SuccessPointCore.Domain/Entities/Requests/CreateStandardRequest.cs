using SuccessPointCore.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace SuccessPointCore.Domain.Entities.Requests
{
    public class CreateStandardRequest
    {
        public int StandardID { get; set; }
        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = MessageConstant.ValidStandardNameRequired)]
        public string StandardName { get; set; }

        public bool IsActive { get; set; }
    }
}
