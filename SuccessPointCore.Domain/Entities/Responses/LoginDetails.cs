namespace SuccessPointCore.Domain.Entities.Responses
{
    public class LoginDetails
    {
        public int UserType { get; set; }
        public string Token { get; set; }
        public DateTime Token_Expires_In { get; set; }
        public Guid Refresh_Token { get; set; }
    }
}
