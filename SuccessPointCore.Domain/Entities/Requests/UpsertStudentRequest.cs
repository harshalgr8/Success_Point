using System.ComponentModel.DataAnnotations;

namespace SuccessPointCore.Domain.Entities.Requests
{
    public class UpsertStudentRequest
    {
        public int StudentID { get; set; }

        [Required(ErrorMessage = "Student Name required")]
        public string StudentName { get; set; }

        [Required(ErrorMessage ="Student username required")]
        public string UserName { get; set; }

        public string Password { get; set; }

        //[Required(ErrorMessage ="Valid Courseid required")]
        public int CourseID { get; set; }

        public string CourseIDCsv { get; set; }

        [Required(ErrorMessage ="Valid StandardID required")]
        public int StandardID { get; set; }

        public bool IsActive { get; set; }

    }
}
