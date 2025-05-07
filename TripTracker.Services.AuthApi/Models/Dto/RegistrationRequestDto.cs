using System.ComponentModel.DataAnnotations;

namespace TripTracker.Services.AuthApi.Models.Dto
{
    public class RegistrationRequestDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Phone number is required")]

        [RegularExpression(@"^\d+$", ErrorMessage = "Phone number must contain only digits")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Travel group name is required")]
        public string? TravelGroupName { get; set; }
        public string? Role { get; set; }
        public bool IsNewGroup { get; set; } = false;
        public int? TravelGroupId { get; set; } = 0;
    }
}