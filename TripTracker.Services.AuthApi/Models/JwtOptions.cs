namespace TripTracker.Services.AuthApi.Models
{
    public class JwtOptions
    {
        public string? Issuer { get; set; } = String.Empty;
        public string? Audience { get; set; } = String.Empty;
        public string? Secret { get; set; } = String.Empty;
        public string? ExpirationInMinutes { get; set; } = String.Empty;
    }
}
