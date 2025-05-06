using TripTracker.Web.Models.Dto;

namespace TripTracker.Web.Service.Interface
{
    public interface IAuthService
    {
        Task<ResponseDto?> Login(LoginRequestDto loginRequest);
        Task<ResponseDto?> Register(RegistrationRequestDto registrationRequestDto);
        Task<ResponseDto?> AssignRole(RegistrationRequestDto registrationRequestDto);
        Task<ResponseDto?> Logout();

    }
}
