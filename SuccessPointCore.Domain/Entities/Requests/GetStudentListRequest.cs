namespace SuccessPointCore.Domain.Entities.Requests
{
    public class GetStudentListRequest
    {
        public int PageSize { get; set; }
        public int PageNo { get; set; }
        public string? StudentName { get; set; }
    }
}
