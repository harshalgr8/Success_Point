namespace SuccessPointCore.Domain.Entities.Requests
{
    public class UpdateStudentRequest : CreateUserRequest
    {
        public int StudentID { get; set; }

        public int StandardID { get; set; }
        public int CourseID { get; set; }
        public string CourseIDCsv { get; set; }
    }
}
