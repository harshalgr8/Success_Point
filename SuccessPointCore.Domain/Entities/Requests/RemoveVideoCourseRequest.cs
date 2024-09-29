using System.ComponentModel.DataAnnotations;

namespace SuccessPointCore.Domain.Entities.Requests
{
    public class RemoveVideoCourseRequest
    {
        [Required(ErrorMessage = "Standard Required")]
        public int StandardId { get; set; }

        [Required(ErrorMessage ="Course Required")]
        public int CourseId { get; set; }
    }
}
