using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TripTracker.Services.AuthApi.Data;
using TripTracker.Services.AuthApi.Models;
using TripTracker.Services.AuthApi.Models.Dto;
using TripTracker.Services.AuthApi.Service.Interface;

namespace TripTracker.Services.AuthApi.Service
{
    public class AuthService : IAuthService
    {
        private readonly AuthDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;
        private readonly ITravelGroupApiService _travelGroupService;
        public AuthService(AuthDbContext db,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IJwtTokenGenerator jwtTokenGenerator,
            IMapper mapper, ITravelGroupApiService travelGroupApiService)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _mapper = mapper;
            _travelGroupService = travelGroupApiService;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;
        }

        public async Task<int> GetTravelGroupId(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                return user.TravelGroupId ?? 0;
            }
            return 0;
        }

        public async Task<UserDto> GetUserById(string userId)
        {
            var user = _mapper.Map<UserDto>(
                    await _userManager.FindByIdAsync(userId));

            if (user != null)
            {
                return user;
            }

            return new UserDto();
        }

        public async Task<IEnumerable<UserDto>> GetUsersByTravelGroup(int travelGroupId)
        {
            if (travelGroupId > 0)
            {
                var users = _mapper.Map<IEnumerable<UserDto>>(
                    await _db.ApplicationUsers.Where(u => u.TravelGroupId == travelGroupId).ToListAsync());

                return users;
            }
            return new List<UserDto>();
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = await _userManager.FindByEmailAsync(loginRequestDto.UserName!);
            if (user == null)
            {
                return new LoginResponseDto { User = null, Token = "" };
            }

            var isValidPassword = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password!);
            if (!isValidPassword)
            {
                return new LoginResponseDto { User = null, Token = "" };
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateToken(user, roles);

            UserDto userDto = new()
            {
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Name = user.Name
            };

            LoginResponseDto loginResponseDto = new LoginResponseDto
            {
                User = userDto,
                Token = token
            };

            return loginResponseDto;
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {

            var travelGroup = await _travelGroupService.GetTravelGroupByName(registrationRequestDto.TravelGroupName!);
            
            var travelGroupExists = travelGroup != null && travelGroup.Id > 0;
            bool requestingNewGroup = registrationRequestDto.IsNewGroup;

            if (travelGroupExists && requestingNewGroup)
            {
                return "Travel group name already registered";
            }

            if (!travelGroupExists && !requestingNewGroup)
            {
                return "Travel group not found.";
            }

            if (!travelGroupExists)
            {
                var travelGroupCreateResponse = await _travelGroupService.CreateAsync(new TravelGroupDto
                {
                    Name = registrationRequestDto.TravelGroupName,
                });
                if (travelGroupCreateResponse?.IsSuccess == false)
                {
                    return travelGroupCreateResponse.Message;
                }
                travelGroup = JsonConvert.DeserializeObject<TravelGroupDto>(Convert.ToString(travelGroupCreateResponse?.Result));
            }

            registrationRequestDto.Role = travelGroupExists ? "MEMBER" : "ADMIN";

            ApplicationUser user = new()
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email!.ToUpper(),
                PhoneNumber = registrationRequestDto.PhoneNumber,
                Name = registrationRequestDto.Name,
                TravelGroupId = travelGroup!.Id,
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDto.Password!);


                if (result.Succeeded)
                {
                    var assignRole = await AssignRole(registrationRequestDto.Email, registrationRequestDto.Role);

                    if (!assignRole)
                    {
                        return "User registered but failed to assign role";
                    }

                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault()!.Description;
                }
            }
            catch (Exception)
            {
                return "Unexpected error occured during registration";
            }
        }
    }
}
