namespace SucessPointCore.Domain.Entities
{
    public class AuthenticatedUser
    {
        public int UserID { get; set; }
        public int UserType { get; set; }
        public bool Active { get; set; }
    }
}
