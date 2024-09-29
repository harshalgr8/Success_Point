using System.ComponentModel.DataAnnotations;

namespace SuccessPointCore.Domain.Entities.Requests
{
    public class VideoCourse
    {
        public string VideoName { get; set; }
        public int VideoID { get; set; }

        [Required(ErrorMessage ="Course Required")]
        public int CourseID { get; set; }

        [Required(ErrorMessage ="Standard Required")]
        public int StandardID { get; set; }
        public int Order { get; set; }
        public string VideoHeading { get; set; }

        public string CourseName { get; set; }
        public string StandardName { get; set; }
    }
}
