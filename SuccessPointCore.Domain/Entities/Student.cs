using System.ComponentModel;

namespace SuccessPointCore.Domain.Entities
{
    public class Student
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public string StandardName { get; set; }

        [DefaultValue("")]
        public string CourseName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }

        [DefaultValue("")]
        public string CourseIDCsv { get; set; }
    }
}
