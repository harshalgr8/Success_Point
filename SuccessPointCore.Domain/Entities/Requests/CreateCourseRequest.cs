using System.ComponentModel.DataAnnotations;

namespace SuccessPointCore.Domain.Entities.Requests
{
    public class CreateCourseRequest
    {
        public int CourseID { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 3, ErrorMessage = Constants.MessageConstant.ValidCourseNameRequired)]
        public string CourseName { get; set; }

        public bool IsActive { get; set; }
    }
}
