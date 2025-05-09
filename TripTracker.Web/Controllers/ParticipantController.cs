using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using TripTracker.Web.Models.Dto;
using TripTracker.Web.Service.Interface;
using TripTracker.Web.ViewModel;

namespace TripTracker.Web.Controllers
{
    [Authorize]
    public class ParticipantController : Controller
    {
        private readonly IParticipantService _participantService;
        private readonly IAuthService _authService;

        public ParticipantController(IParticipantService participantService, IAuthService authService)
        {
            _participantService = participantService;
            _authService = authService;
        }

        public async Task<IActionResult> Index()
        {
            List<ParticipantDto> participants = new();

            var response = await _participantService.GetAllAsync();

            if (response?.IsSuccess == true && response.Result != null)
            {
                try
                {
                    participants = JsonConvert
                        .DeserializeObject<List<ParticipantDto>>(Convert.ToString(response.Result))
                        ?? new List<ParticipantDto>();
                }
                catch (JsonException ex)
                {
                    TempData["error"] = $"Error parsing data: {ex.Message}";
                    ModelState.AddModelError(string.Empty, $"Error parsing data: {ex.Message}");
                }
            }
            else
            {
                TempData["error"] = response?.Message ?? "Failed to retrieve participants.";
                ModelState.AddModelError(string.Empty, response?.Message ?? "Failed to retrieve participants.");
            }

            return View(participants);
        }

        [HttpGet]
        public async Task<IActionResult> AddParticipants(int tripId)
        {
            var userEmail = User.Identity?.Name;
            var authResponse = await _authService.GetTravelGroupId(userEmail!);

            if (authResponse == null)
            {
                TempData["error"] = "Failed to retrieve travel group ID.";
                ModelState.AddModelError(string.Empty, "Failed to retrieve travel group ID.");
                return View(new AddParticipantViewModel());
            }

            var travelGroupId = authResponse.IsSuccess == true && authResponse.Result != null ?
                JsonConvert.DeserializeObject<int>(Convert.ToString(authResponse.Result)) : 0;

            var userResponse = await _authService.GetUsersByTravelGroup(travelGroupId);

            var users = (userResponse!.IsSuccess == true && userResponse != null) ?
                JsonConvert.DeserializeObject<List<UserDto>>(Convert.ToString(userResponse.Result)) : new List<UserDto>();

            var participantResponse = await _participantService.GetAllByTripAsync(tripId);

            var participants = (participantResponse!.IsSuccess == true && participantResponse != null) ?
                JsonConvert.DeserializeObject<List<ParticipantDto>>(Convert.ToString(participantResponse.Result)) : new List<ParticipantDto>();

            AddParticipantViewModel addParticipantViewModel = new()
            {
                TripId = tripId,
                AvailableUsers = users!
                    .Where(u => !participants.Any(p => p.Email == u.Email))
                    .Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    })

            };
            return View(addParticipantViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddParticipants(AddParticipantViewModel addParticipantViewModel)
        {
            if (!addParticipantViewModel.SelectedUserIds.Any())
            {
                TempData["error"] = "No participants selected.";
            }

            bool isCreateParticipantSuccess = false;

            foreach (var userId in addParticipantViewModel.SelectedUserIds)
            {
                var userResponse = await _authService.GetUserById(userId);
                if (userResponse?.IsSuccess == true && userResponse.Result != null)
                {
                    var user = JsonConvert.DeserializeObject<UserDto>(Convert.ToString(userResponse.Result));
                    
                    if(user != null)
                    {
                        var participant = new ParticipantDto
                        {
                            TripId = addParticipantViewModel.TripId,
                            Name = user.Name,
                            Email = user.Email,
                        };
                        var createParticipantResponse = await _participantService.CreateAsync(participant);

                        if (createParticipantResponse?.IsSuccess != true)
                        {
                            isCreateParticipantSuccess = false;
                            TempData["error"] = createParticipantResponse?.Message ?? "Failed to add participant.";
                            break;
                        }
                        else
                        {
                            isCreateParticipantSuccess = true;
                        }
                    }                    
                }
            }

            if (isCreateParticipantSuccess)
            {
                TempData["success"] = "Participants added successfully.";
            }
                      
            return RedirectToAction("Details", "Trips", new { tripId = addParticipantViewModel.TripId });
        }

        //[HttpGet]
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Create(ParticipantDto participantDto)
        //{
        //    if (participantDto == null)
        //    {
        //        TempData["error"] = "Invalid participant data.";
        //        ModelState.AddModelError(string.Empty, "Invalid participant data.");
        //        return View(participantDto);
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        TempData["error"] = "Invalid participant data.";
        //        ModelState.AddModelError(string.Empty, "Invalid participant data.");
        //        return View(participantDto);
        //    }

        //    var response = await _participantService.CreateAsync(participantDto);
        //    if (response?.IsSuccess == true)
        //    {
        //        TempData["success"] = "Participant created successfully.";
        //        return RedirectToAction(nameof(Index));
        //    }

        //    TempData["error"] = response?.Message ?? "Failed to create participant.";
        //    ModelState.AddModelError(string.Empty, response?.Message ?? "Failed to create participant.");
        //    return View(participantDto);
        //}

        //public async Task<IActionResult> Details(int id)
        //{
        //    var response = await _participantService.GetAsync(id);
        //    if (response?.IsSuccess == true && response.Result != null)
        //    {
        //        var participant = JsonConvert.DeserializeObject<ParticipantDto>(Convert.ToString(response.Result));
        //        return View(participant);
        //    }

        //    TempData["error"] = response?.Message ?? "Participant not found.";
        //    ModelState.AddModelError(string.Empty, response?.Message ?? "Participant not found.");
        //    return View(new ParticipantDto());
        //}

        //[HttpGet]
        //public async Task<IActionResult> Edit(int id)
        //{
        //    var response = await _participantService.GetAsync(id);
        //    if (response != null && response?.IsSuccess == true && response.Result != null)
        //    {
        //        var participant = JsonConvert.DeserializeObject<ParticipantDto>(Convert.ToString(response.Result));
        //        return View(participant);
        //    }

        //    TempData["error"] = response?.Message ?? "Participant not found.";
        //    ModelState.AddModelError(string.Empty, response?.Message ?? "Participant not found.");
        //    return View(new ParticipantDto());
        //}

        //[HttpPost]
        //public async Task<IActionResult> Edit(ParticipantDto participantDto)
        //{
        //    if (participantDto == null)
        //    {
        //        TempData["error"] = "Invalid participant data.";
        //        ModelState.AddModelError(string.Empty, "Invalid participant data.");
        //        return View(participantDto);
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        TempData["error"] = "Invalid participant data.";
        //        ModelState.AddModelError(string.Empty, "Invalid participant data.");
        //        return View(participantDto);
        //    }

        //    var response = await _participantService.UpdateAsync(participantDto);
        //    if (response?.IsSuccess == true)
        //    {
        //        TempData["success"] = "Participant updated successfully.";
        //        return RedirectToAction(nameof(Index));
        //    }

        //    TempData["error"] = response?.Message ?? "Failed to update participant.";
        //    ModelState.AddModelError(string.Empty, response?.Message ?? "Failed to update participant.");
        //    return View(participantDto);
        //}


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _participantService.GetAsync(id);
            if (response?.IsSuccess == true && response.Result != null)
            {
                var participant = JsonConvert.DeserializeObject<ParticipantDto>(Convert.ToString(response.Result));
                return View(participant);
            }

            TempData["error"] = response?.Message ?? "Participant not found.";
            ModelState.AddModelError(string.Empty, response?.Message ?? "Participant not found.");
            return View(new ParticipantDto());
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _participantService.DeleteAsync(id);
            if (response?.IsSuccess == true)
            {
                TempData["success"] = "Participant deleted successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = response?.Message ?? "Failed to delete participant.";
            ModelState.AddModelError(string.Empty, response?.Message ?? "Failed to delete participant.");
            return RedirectToAction("Delete", "Participant", new { id });
        }
    }
}
