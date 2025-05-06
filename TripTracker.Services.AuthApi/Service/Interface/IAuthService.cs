using TripTracker.Services.AuthApi.Model.Dto;

namespace TripTracker.Services.AuthApi.Service.Interface
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDto registrationRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<bool> AssignRole(string email, string roleName);
    }
}
