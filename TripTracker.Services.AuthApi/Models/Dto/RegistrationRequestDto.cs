using System.ComponentModel.DataAnnotations;

namespace TripTracker.Services.AuthApi.Models.Dto
{
    public class RegistrationRequestDto
    {
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; set; }
        public string? Role { get; set; }

    }
}