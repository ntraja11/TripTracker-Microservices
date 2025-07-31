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

            List<ParticipantDto> participants = (participantResponse!.IsSuccess == true && participantResponse != null) ?
                JsonConvert.DeserializeObject<List<ParticipantDto>>(Convert.ToString(participantResponse.Result)) : new List<ParticipantDto>();

            AddParticipantViewModel addParticipantViewModel = new()
            {
                TripId = tripId,
                SelectedUsers = participants!.Select(p => new SelectListItem { Text = p.Name, Value = p.Id.ToString() }),
                SelectedUserIds = participants!.Select(p => p.Id.ToString()).ToList(),
                AvailableUsers = users!
                    .Where(u => !participants!.Any(p => p.Email == u.Email))
                    .Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id!.ToString()
                    })

            };
            return View(addParticipantViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddParticipants(AddParticipantViewModel addParticipantViewModel)
        {
            var existingParticipantsResponse = await _participantService.GetAllByTripAsync(addParticipantViewModel.TripId);
            if (existingParticipantsResponse == null || !existingParticipantsResponse.IsSuccess)
            {
                TempData["error"] = "Failed to retrieve existing participants.";
                return RedirectToAction("Details", "Trips", new { tripId = addParticipantViewModel.TripId });
            }

            var existingParticipants = (existingParticipantsResponse?.IsSuccess == true && existingParticipantsResponse.Result != null)
                ? JsonConvert.DeserializeObject<List<ParticipantDto>>(Convert.ToString(existingParticipantsResponse.Result))
                : new List<ParticipantDto>();



            if (!addParticipantViewModel.SelectedUserIds.Any() && existingParticipants!.Count == 0)
            {
                TempData["error"] = "No participants selected.";
                return RedirectToAction("Details", "Trips", new { tripId = addParticipantViewModel.TripId });
            }

            // Get the current participants from the database
            
            

            var existingParticipantIds = existingParticipants.Select(p => p.Id.ToString()).ToList(); // IDs currently in DB
            var newParticipantIds = addParticipantViewModel.SelectedUserIds; // IDs submitted from the form

            bool isParticipantUpdated = false;



            // **Step 1: Remove Unselected Participants**
            var participantsToRemove = existingParticipantIds
                        .Where(id => !newParticipantIds.Contains(id!))
                        .ToList();
            foreach (var participantId in participantsToRemove)
            {
                var removeResponse = await _participantService.DeleteAsync(int.Parse(participantId!));
                if (removeResponse?.IsSuccess == true)
                {
                    isParticipantUpdated = true;
                }
                else
                {
                    isParticipantUpdated = false;
                    TempData["error"] = removeResponse?.Message ?? $"Failed to remove participant {participantId}.";
                    break;
                }
            }

            var participantsToAdd = newParticipantIds
                .Where(id => !existingParticipantIds.Contains(id))
                .ToList();

            // ✅ **Step 2: Add New Participants**
            foreach (var userId in participantsToAdd)
            {
                var userResponse = await _authService.GetUserById(userId.ToString());
                if (userResponse?.IsSuccess == true && userResponse.Result != null)
                {
                    var user = JsonConvert.DeserializeObject<UserDto>(Convert.ToString(userResponse.Result));

                    if (user != null)
                    {
                        var participant = new ParticipantDto
                        {
                            TripId = addParticipantViewModel.TripId,
                            Name = user.Name,
                            Email = user.Email,
                        };

                        var createParticipantResponse = await _participantService.CreateAsync(participant);
                        if (createParticipantResponse?.IsSuccess == true)
                        {
                            isParticipantUpdated = true;
                        }
                        else
                        {
                            isParticipantUpdated = false;
                            TempData["error"] = createParticipantResponse?.Message ?? "Failed to add participant.";
                        }
                    }
                }
            }

            if (isParticipantUpdated)
            {
                TempData["success"] = "Participants updated successfully.";
            }

            return RedirectToAction("Details", "Trips", new { tripId = addParticipantViewModel.TripId });
        }


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
