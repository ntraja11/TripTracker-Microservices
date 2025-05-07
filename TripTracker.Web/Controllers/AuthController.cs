using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using TripTracker.Web.Models.Dto;
using TripTracker.Web.Service.Interface;
using TripTracker.Web.Utility;

namespace TripTracker.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITravelGroupService _travelGroupService;
        private readonly ITokenHandler _tokenHandler;

        public AuthController(IAuthService authService,  ITravelGroupService travelGroupService, ITokenHandler tokenHandler)
        {
            _authService = authService;
            _travelGroupService = travelGroupService;
            _tokenHandler = tokenHandler;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new LoginRequestDto();
            return View(loginRequestDto);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Invalid login data";
                ModelState.AddModelError(string.Empty, "Invalid login data.");
                return View();
            }

            var responseDto = await _authService.LoginAsync(loginRequestDto);

            if (responseDto?.IsSuccess == true)
            {
                LoginResponseDto loginResponseDto = JsonConvert
                    .DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result))!;


                await SignInUserAsync(loginResponseDto);
                _tokenHandler.SetToken(loginResponseDto.Token!);

                TempData["success"] = "Login successful.";
                return RedirectToAction("Index", "Home");
            }

            TempData["error"] = responseDto!.Message;
            ModelState.AddModelError(string.Empty, responseDto!.Message);
            return View(loginRequestDto);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto registrationRequestDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Invalid registration data.";
                ModelState.AddModelError(string.Empty, "Invalid registration data.");
                return View();
            }

            var travelGroupResponse = await _travelGroupService.GetAsync(registrationRequestDto.TravelGroupName!);

            bool travelGroupExists = travelGroupResponse?.IsSuccess == true;
            bool requestNewGroup = registrationRequestDto.IsNewGroup;

            if (travelGroupExists && requestNewGroup)
            {
                TempData["error"] = "Travel group already exists.";
                ModelState.AddModelError(string.Empty, "Travel group already exists.");
                return View();
            }

            if (!travelGroupExists && !requestNewGroup)
            {
                TempData["error"] = "Travel group not found.";
                ModelState.AddModelError(string.Empty, "Travel group not found.");
                return View();
            }

           
            ResponseDto? travelGroupCreateResponse = null;

            if (!travelGroupExists)
            {
                travelGroupCreateResponse = await _travelGroupService.CreateAsync(new TravelGroupDto
                {
                    Name = registrationRequestDto.TravelGroupName,
                });

                if (travelGroupCreateResponse?.IsSuccess == false)
                {
                    TempData["error"] = travelGroupCreateResponse.Message;
                    ModelState.AddModelError(string.Empty, travelGroupCreateResponse.Message);
                    return View();
                }

            }
           

            registrationRequestDto.TravelGroupId = JsonConvert.DeserializeObject<TravelGroupDto>(Convert.ToString(travelGroupCreateResponse?.Result))?.Id
                                                    ?? JsonConvert.DeserializeObject<TravelGroupDto>(Convert.ToString(travelGroupResponse?.Result))?.Id;
            
            registrationRequestDto.Role = travelGroupExists ? StaticDetail.RoleMember 
                                            : StaticDetail.RoleAdmin;


            var registrationResponseDto = await _authService.RegisterAsync(registrationRequestDto);

            if (registrationResponseDto?.IsSuccess == true)
            {
                var assignRole = await _authService.AssignRoleAsync(registrationRequestDto);

                if (assignRole?.IsSuccess == false)
                {
                    var errorMessage = assignRole?.Message ?? "Failed to assign role";
                    TempData["error"] = errorMessage;
                    ModelState.AddModelError(string.Empty, errorMessage);
                    return View();
                }
                TempData["success"] = "User registered successfully.";
                return RedirectToAction(nameof(Login));
            }
            else
            {
                TempData["error"] = registrationResponseDto!.Message;
                ModelState.AddModelError(string.Empty, registrationResponseDto!.Message);
                return View();
            }
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            _tokenHandler.RemoveToken();

            return RedirectToAction(nameof(Login));

        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private async Task SignInUserAsync(LoginResponseDto loginResponseDto)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwtToken = handler.ReadJwtToken(loginResponseDto.Token);

            var claimList = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            claimList.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email).Value));
            claimList.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub).Value));
            claimList.AddClaim(new Claim(JwtRegisteredClaimNames.UniqueName,
                jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName).Value));


            claimList.AddClaim(new Claim(ClaimTypes.Name,
                jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email).Value));
            claimList.AddClaim(new Claim(ClaimTypes.Role,
                jwtToken.Claims.FirstOrDefault(c => c.Type == "role").Value));

            var principal = new ClaimsPrincipal(claimList);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        }
    }
}
