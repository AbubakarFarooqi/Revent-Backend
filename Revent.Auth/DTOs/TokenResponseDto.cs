namespace Revent.Auth.DTOs
{
    public class TokenResponseDto
    {
        public string? access_token { get; set; }
        public string? refresh_token { get; set; }
        public int? expires_in { get; set; }
    }
}
