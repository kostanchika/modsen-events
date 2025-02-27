namespace EventsAPI.BLL.Models
{
    public class RefreshTokenModel
    {
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
