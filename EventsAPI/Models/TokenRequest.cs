namespace EventsAPI.Models
{
    public record class TokenRequest(string AccessToken, string RefreshToken);
}
