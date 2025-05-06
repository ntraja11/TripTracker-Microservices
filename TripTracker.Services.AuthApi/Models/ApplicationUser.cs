using Microsoft.AspNetCore.Identity;

namespace TripTracker.Services.AuthApi.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
    }
}
