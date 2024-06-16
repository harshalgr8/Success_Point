namespace SucessPointCore.Domain.Entities
{
    public class CreateUser : UpdatePassword
    {
        public string Email { get; set; }
        public int MobileNo { get; set; }
        public bool Active { get; set; }
    }
}
