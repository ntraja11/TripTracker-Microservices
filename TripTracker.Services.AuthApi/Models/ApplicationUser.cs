using Microsoft.AspNetCore.Identity;

namespace TripTracker.Services.AuthApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public int? TravelGroupId { get; set; }
    }
}
