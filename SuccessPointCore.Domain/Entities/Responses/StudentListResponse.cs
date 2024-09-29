namespace SuccessPointCore.Domain.Entities.Responses
{
    public class StudentListResponse
    {
        public int TotalCount { get; set; }
        public IEnumerable<Student> Students { get; set; }
    }
}
