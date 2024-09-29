using System.ComponentModel.DataAnnotations;

namespace SuccessPointCore.Domain.Entities.Requests
{
    public class RemoveStandardRequest
    {
        [Required(ErrorMessage ="Valid StandardID Required.")]
        public int StandardID { get; set; }
    }
}
