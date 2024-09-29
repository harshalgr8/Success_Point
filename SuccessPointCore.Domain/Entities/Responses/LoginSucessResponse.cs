namespace SuccessPointCore.Domain.Entities.Responses
{
    public class LoginSucessResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public object Details { get; set; }
        //public int UserType { get; set; }
        //public string Token { get; set; }
        //public DateTime Token_Expires_In { get; set; }
        //public Guid Refresh_Token { get; set; }
    }
}
