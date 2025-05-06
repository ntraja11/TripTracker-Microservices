using TripTracker.Services.AuthApi.Models;

namespace TripTracker.Services.AuthApi.Service.Interface
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser applicationUser);
    }
}
