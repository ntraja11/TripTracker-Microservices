using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using TripTracker.Web.Models;
using TripTracker.Web.Models.Dto;
using TripTracker.Web.Service.Interface;
using TripTracker.Web.Utility;
using TripTracker.Web.ViewModel;

namespace TripTracker.Web.Controllers
{
    [Authorize]
    public class TripsController : Controller
    {
        private readonly ITripService _tripService;
        private readonly IParticipantService _participantService;
        private readonly IExpenseService _expenseService;
        private readonly IAuthService _authService;

        public TripsController(ITripService tripService, IExpenseService expenseService, 
            IAuthService authService, IParticipantService participantService)
        {
            _tripService = tripService;
            _authService = authService;
            _expenseService = expenseService;
            _participantService = participantService;
        }

        public async Task<IActionResult> Index()
        {
            List<TripDto> trips = new();
            
            int travelGroupId = await GetTravelGroupId();

            var response = await _tripService.GetByTravelGroupAsync(travelGroupId);

            if (response?.IsSuccess == true && response.Result != null)
            {
                try
                {
                    trips = JsonConvert
                        .DeserializeObject<List<TripDto>>(Convert.ToString(response.Result))
                        ?? new List<TripDto>();
                }
                catch (JsonException ex)
                {
                    TempData["error"] = $"Error parsing data: {ex.Message}";
                    ModelState.AddModelError(string.Empty, $"Error parsing data: {ex.Message}");
                }
            }
            else
            {
                TempData["error"] = response?.Message ?? "Failed to retrieve trips.";
                ModelState.AddModelError(string.Empty, response?.Message ?? "Failed to retrieve trips.");
            }

            return View(trips);
        }

        private async Task<int> GetTravelGroupId()
        {
            var email = HttpContext.User.Identity!.Name;
            var travelGroupId = 0;
            var authResponse = await _authService.GetTravelGroupId(email!);

            if (authResponse?.IsSuccess == true && authResponse.Result != null)
            {
                travelGroupId = JsonConvert.DeserializeObject<int>(Convert.ToString(authResponse.Result));
            }

            return travelGroupId;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TripDto tripDto)
        {
            if (tripDto == null || !ModelState.IsValid)
            {
                TempData["error"] = "Invalid trip data.";
                ModelState.AddModelError(string.Empty, "Invalid trip data.");
                return View(tripDto);
            }

            int travelGroupId = await GetTravelGroupId();
            tripDto.TravelGroupId = travelGroupId;

            if(tripDto.StartDate != null)
            {

                tripDto.Status = (tripDto.StartDate > DateOnly.FromDateTime(DateTime.Now)) 
                    ? StaticDetail.TripStatusPlanned : StaticDetail.TripStatusInProgress;
            }
            else
            {
                tripDto.Status = StaticDetail.TripStatusPlanned;
            }

                var response = await _tripService.CreateAsync(tripDto);
            if (response?.IsSuccess == true)
            {
                TempData["success"] = "Trip created successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = response?.Message ?? "Failed to create trip.";
            ModelState.AddModelError(string.Empty, response?.Message ?? "Failed to create trip.");
            return View(tripDto);
        }

        public async Task<IActionResult> Details(int tripId)
        {
            var response = await _tripService.GetAsync(tripId);
            if (response?.IsSuccess == true && response.Result != null)
            {
                var trip = JsonConvert.DeserializeObject<TripDto>(Convert.ToString(response.Result));
                               
                var participantsResponse = await _participantService.GetAllByTripAsync(tripId);

                var participants = (participantsResponse!.IsSuccess == true && participantsResponse != null) ?
                    JsonConvert.DeserializeObject<List<ParticipantDto>>(Convert.ToString(participantsResponse.Result)) : new List<ParticipantDto>();

                var expenseResponse = await _expenseService.GetAllByTripAsync(tripId);

                var expenses = (expenseResponse!.IsSuccess == true && expenseResponse != null) ?
                    JsonConvert.DeserializeObject<List<ExpenseDto>>(Convert.ToString(expenseResponse.Result)) : new List<ExpenseDto>();

                decimal tripTotalExpense = 0;

                foreach(var participant in participants)
                {
                    participant.TotalTripExpense = expenses.Where(e => e.ParticipantId == participant.Id)
                        .Sum(e => e.Amount);
                    tripTotalExpense += (decimal) participant.TotalTripExpense;
                }

                trip.TotalExpense = tripTotalExpense;
                var participantShare = (tripTotalExpense > 0 && participants.Count > 0) ? tripTotalExpense / participants.Count : 0;

                TripDetailViewModel tripDetailViewModel = new()
                {                    
                    Trip = trip!,
                    Participants = participants!,
                    Expenses = expenses!,
                    ParticipantShare = participantShare

                };

                return View(tripDetailViewModel);
            }

            TempData["error"] = response?.Message ?? "Trip not found.";
            ModelState.AddModelError(string.Empty, response?.Message ?? "Trip not found.");


            return View(new TripDto());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int tripId)
        {
            var response = await _tripService.GetAsync(tripId);
            if (response != null && response?.IsSuccess == true && response.Result != null)
            {
                var trip = JsonConvert.DeserializeObject<TripDto>(Convert.ToString(response.Result));
                return View(trip);
            }

            TempData["error"] = response?.Message ?? "Trip not found.";
            ModelState.AddModelError(string.Empty, response?.Message ?? "Trip not found.");
            return View(new TripDto());
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TripDto tripDto)
        {
            if (tripDto == null || !ModelState.IsValid)
            {
                TempData["error"] = "Invalid trip data.";
                ModelState.AddModelError(string.Empty, "Invalid trip data.");
                return View(tripDto);
            }


            if(tripDto.StartDate != null && tripDto.EndDate == null)
            {
                tripDto.Status = (tripDto.StartDate > DateOnly.FromDateTime(DateTime.Now))
                    ? StaticDetail.TripStatusPlanned : StaticDetail.TripStatusInProgress;
            }


            if(tripDto.EndDate != null && tripDto.StartDate != null)
            {
                if(tripDto.EndDate < tripDto.StartDate)
                {
                    TempData["error"] = "End date cannot be earlier than start date.";
                    ModelState.AddModelError(string.Empty, "End date cannot be earlier than start date.");
                    return View(tripDto);
                }
                else if (tripDto.EndDate > DateOnly.FromDateTime(DateTime.Now))
                {
                    TempData["error"] = "End date cannot be later than today.";
                    ModelState.AddModelError(string.Empty, "End date cannot be later than today.");
                    return View(tripDto);
                }
                else
                {
                    tripDto.Status = StaticDetail.TripStatusCompleted;
                }
            }          



            var response = await _tripService.UpdateAsync(tripDto);
            if (response?.IsSuccess == true)
            {
                TempData["success"] = "Trip updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = response?.Message ?? "Failed to update trip.";
            ModelState.AddModelError(string.Empty, response?.Message ?? "Failed to update trip.");
            return View(tripDto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int tripId)
        {
            var response = await _tripService.GetAsync(tripId);
            if (response?.IsSuccess == true && response.Result != null)
            {
                var trip = JsonConvert.DeserializeObject<TripDto>(Convert.ToString(response.Result));
                return View(trip);
            }

            TempData["error"] = response?.Message ?? "Trip not found.";
            ModelState.AddModelError(string.Empty, response?.Message ?? "Trip not found.");
            return View(new TripDto());
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int tripId)
        {
            var response = await _tripService.DeleteAsync(tripId);
            if (response?.IsSuccess == true)
            {
                TempData["success"] = "Trip deleted successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = response?.Message ?? "Failed to delete trip.";
            ModelState.AddModelError(string.Empty, response?.Message ?? "Failed to delete trip.");
            return RedirectToAction("Delete", "Trip", new { tripId });
        }
    }
}
