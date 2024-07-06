using System.ComponentModel.DataAnnotations;

namespace SucessPointCore.Domain.Entities.Requests
{
    public class CreateCourseRequest
    {
        [Required]
        [StringLength(200, MinimumLength = 3, ErrorMessage = Constants.MessageConstant.ValidCourseNameRequired)]
        public string CourseName { get; set; }
    }
}
