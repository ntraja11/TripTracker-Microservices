using TripTracker.Services.AuthApi.Models.Dto;

namespace TripTracker.Services.AuthApi.Service.Interface
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDto registrationRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<bool> AssignRole(string email, string roleName);

        Task<int> GetTravelGroupId(string email);
        Task<IEnumerable<UserDto>> GetUsersByTravelGroup(int travelGroupId);
        Task<UserDto> GetUserById(string userId);
    }
}
