namespace TripTracker.Services.AuthApi.Model.Dto
{
    public class LoginResponseDto
    {
        public string? Token { get; set; }
        public UserDto? User { get; set; }
    }
}
