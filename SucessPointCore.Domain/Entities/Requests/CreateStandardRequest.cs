using System.ComponentModel.DataAnnotations;

namespace SucessPointCore.Domain.Entities.Requests
{
    public class CreateStandardRequest
    {
        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "Valid Standard name requried")]
        public string StandardName { get; set; }
    }
}
