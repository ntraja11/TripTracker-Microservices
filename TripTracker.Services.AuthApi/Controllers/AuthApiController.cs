using Microsoft.AspNetCore.Mvc;
using TripTracker.Services.AuthApi.Models.Dto;
using TripTracker.Services.AuthApi.Service.Interface;

namespace TripTracker.Services.AuthApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthApiController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected ResponseDto _response;

        public AuthApiController(IAuthService authService, ResponseDto responseDto)
        {
            _authService = authService;
            _response = responseDto;
        }

        [HttpGet]
        [Route("GetTravelGroupId/{email}")]
        public async Task<IActionResult> GetTravelGroupId(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                _response.IsSuccess = false;
                _response.Message = "Email is required";
                return BadRequest(_response);
            }
            var travelGroupId = await _authService.GetTravelGroupId(email);
            if (travelGroupId == 0)
            {
                _response.IsSuccess = false;
                _response.Message = "User or travel group not found";
                return NotFound(_response);
            }
            _response.Result = travelGroupId;
            return Ok(_response);
        }

        [HttpGet]
        [Route("GetUserById/{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _response.IsSuccess = false;
                _response.Message = "Invalid user ID";
                return BadRequest(_response);
            }

            var user = await _authService.GetUserById(userId);

            _response.Result = user;
            return Ok(_response);
        }

        [HttpGet]
        [Route("GetUsersByTravelGroup/{travelGroupId:int}")]
        public async Task<IActionResult> GetUsersByTravelGroup(int travelGroupId)
        {
            if (travelGroupId == 0)
            {
                _response.IsSuccess = false;
                _response.Message = "Invalid travel group ID";
                return BadRequest(_response);
            }

            var users = await _authService.GetUsersByTravelGroup(travelGroupId);

            _response.Result = users;
            return Ok(_response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto registrationRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var errorMessage = await _authService.Register(registrationRequestDto);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _response.IsSuccess = false;
                _response.Message = errorMessage;
                return BadRequest(_response);
            }
            return Ok(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var loginResponseDto = await _authService.Login(loginRequestDto);

            if (loginResponseDto.User == null || string.IsNullOrEmpty(loginResponseDto.Token))
            {
                _response.IsSuccess = false;
                _response.Message = "Username or password is incorrect";
                return BadRequest(_response);
            }
            _response.Result = loginResponseDto;

            return Ok(_response);
        }

        [HttpPost("assignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto registrationRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isRoleAssigned = await _authService
                .AssignRole(registrationRequestDto.Email!, registrationRequestDto.Role!.ToUpper());

            if (!isRoleAssigned)
            {
                _response.IsSuccess = false;
                _response.Message = "Failed to assign role. Either user does not exist or role is invalid.";
                return BadRequest(_response);
            }

            return Ok(_response);
        }
    }
}
