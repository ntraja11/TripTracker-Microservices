using TripTracker.Web.Models.Dto;

namespace TripTracker.Web.Service.Interface
{
    public interface IAuthService
    {
        Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequest);
        Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registrationRequestDto);
        Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto registrationRequestDto);
        Task<ResponseDto?> Logout();
        Task<ResponseDto?> GetTravelGroupId(string email);
        Task<ResponseDto?> GetUsersByTravelGroup(int travelGroupId);
        Task<ResponseDto?> GetUserById(string userId);


    }
}
