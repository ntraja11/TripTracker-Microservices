using TripTracker.Web.Models.Dto;
using TripTracker.Web.Service.Interface;
using static TripTracker.Web.Utility.StaticDetail;

namespace TripTracker.Web.Service
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;
        private readonly string AuthApiPath = "";
        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
            AuthApiPath = AuthApiBasePath + "/api/auth";
        }
        public async Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.POST,
                Url = AuthApiPath + "/assignRole",
                Data = registrationRequestDto
            });
        }

        public async Task<ResponseDto?> GetTravelGroupId(string email)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.GET,
                Url = AuthApiPath + "/getTravelGroupId/" + email,
            });
        }

        public async Task<ResponseDto?> GetUserById(string userId)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.GET,
                Url = AuthApiPath + "/GetUserById/" + userId,
            });
        }

        public async Task<ResponseDto?> GetUsersByTravelGroup(int travelGroupId)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.GET,
                Url = AuthApiPath + "/GetUsersByTravelGroup/" + travelGroupId,
            });
        }

        public async Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.POST,
                Url = AuthApiPath + "/login",
                Data = loginRequestDto
            }, withBearer: false);
        }

        public Task<ResponseDto?> Logout()
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.POST,
                Url = AuthApiPath + "/Register",
                Data = registrationRequestDto
            }, withBearer: false);
        }

    }
}
