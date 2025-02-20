namespace EventsAPI.Models
{
    public class RefreshTokenModel
    {
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
