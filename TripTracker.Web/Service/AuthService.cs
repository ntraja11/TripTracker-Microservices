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
            AuthApiPath = AuthApiBasePath + "/api/auth/";
        }
        public async Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.POST,
                Url = AuthApiPath + "assign-role",
                Data = registrationRequestDto
            });
        }

        public async Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.POST,
                Url = AuthApiPath + "login",
                Data = loginRequestDto
            });
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
                Url = AuthApiPath + "register",
                Data = registrationRequestDto
            });
        }
    }
}
