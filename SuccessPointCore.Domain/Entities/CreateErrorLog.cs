namespace SuccessPointCore.Domain.Entities
{
    public class CreateErrorLog
    {
        public string ErrorMesage { get; set; }
        public string StackTrace { get; set; }
        public int? UserID { get; set; }
    }
}
