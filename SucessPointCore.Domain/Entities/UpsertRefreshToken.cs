namespace SucessPointCore.Domain.Entities
{
    public class UpsertRefreshToken
    {
        public int UserID { get; set; }
        public Guid RefreshToken { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
