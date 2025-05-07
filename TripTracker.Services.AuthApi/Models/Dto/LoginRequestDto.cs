using System.ComponentModel.DataAnnotations;

namespace TripTracker.Services.AuthApi.Models.Dto
{
    public class LoginRequestDto
    {
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
