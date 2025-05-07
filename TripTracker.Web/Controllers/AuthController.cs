using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
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

        public AuthController(IAuthService authService, ITravelGroupService travelGroupService)
        {
            _authService = authService;
            _travelGroupService = travelGroupService;
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

            var loginResponseDto = await _authService.LoginAsync(loginRequestDto);

            if (loginResponseDto?.IsSuccess == true)
            {
                TempData["success"] = "Login successful.";
                return RedirectToAction("Index", "Home");
            }

            TempData["error"] = loginResponseDto!.Message;
            ModelState.AddModelError(string.Empty, loginResponseDto!.Message);
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
           

            registrationRequestDto.TravelGroupId = JsonConvert.DeserializeObject<TravelGroupDto>(Convert.ToString(travelGroupCreateResponse.Result))?.Id
                                                    ?? JsonConvert.DeserializeObject<TravelGroupDto>(Convert.ToString(travelGroupResponse.Result))?.Id;
            
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
        public IActionResult Logout()
        {
            return View();
        }
    }
}
