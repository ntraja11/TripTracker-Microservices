using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TripTracker.Web.Models.Dto;
using TripTracker.Web.Service.Interface;

namespace TripTracker.Web.Controllers
{
    [Authorize]
    public class ParticipantController : Controller
    {
        private readonly IParticipantService _participantService;

        public ParticipantController(IParticipantService participantService)
        {
            _participantService = participantService;
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
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ParticipantDto participantDto)
        {
            if (participantDto == null)
            {
                TempData["error"] = "Invalid participant data.";
                ModelState.AddModelError(string.Empty, "Invalid participant data.");
                return View(participantDto);
            }

            if (!ModelState.IsValid)
            {
                TempData["error"] = "Invalid participant data.";
                ModelState.AddModelError(string.Empty, "Invalid participant data.");
                return View(participantDto);
            }

            var response = await _participantService.CreateAsync(participantDto);
            if (response?.IsSuccess == true)
            {
                TempData["success"] = "Participant created successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = response?.Message ?? "Failed to create participant.";
            ModelState.AddModelError(string.Empty, response?.Message ?? "Failed to create participant.");
            return View(participantDto);
        }

        public async Task<IActionResult> Details(int id)
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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _participantService.GetAsync(id);
            if (response != null && response?.IsSuccess == true && response.Result != null)
            {
                var participant = JsonConvert.DeserializeObject<ParticipantDto>(Convert.ToString(response.Result));
                return View(participant);
            }

            TempData["error"] = response?.Message ?? "Participant not found.";
            ModelState.AddModelError(string.Empty, response?.Message ?? "Participant not found.");
            return View(new ParticipantDto());
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ParticipantDto participantDto)
        {
            if (participantDto == null)
            {
                TempData["error"] = "Invalid participant data.";
                ModelState.AddModelError(string.Empty, "Invalid participant data.");
                return View(participantDto);
            }

            if (!ModelState.IsValid)
            {
                TempData["error"] = "Invalid participant data.";
                ModelState.AddModelError(string.Empty, "Invalid participant data.");
                return View(participantDto);
            }

            var response = await _participantService.UpdateAsync(participantDto);
            if (response?.IsSuccess == true)
            {
                TempData["success"] = "Participant updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = response?.Message ?? "Failed to update participant.";
            ModelState.AddModelError(string.Empty, response?.Message ?? "Failed to update participant.");
            return View(participantDto);
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
