using TripTracker.Services.AuthApi.Model;

namespace TripTracker.Services.AuthApi.Service.Interface
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser applicationUser);
    }
}
