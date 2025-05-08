using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TripTracker.Web.Models.Dto;
using TripTracker.Web.Service.Interface;

namespace TripTracker.Web.Controllers
{
    [Authorize]
    public class TripController : Controller
    {
        private readonly ITripService _tripService;

        public TripController(ITripService tripService)
        {
            _tripService = tripService;
        }

        public async Task<IActionResult> Index()
        {
            List<TripDto> trips = new();

            var response = await _tripService.GetAllAsync();

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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TripDto tripDto)
        {
            if (tripDto == null)
            {
                TempData["error"] = "Invalid trip data.";
                ModelState.AddModelError(string.Empty, "Invalid trip data.");
                return View(tripDto);
            }

            if (!ModelState.IsValid)
            {
                TempData["error"] = "Invalid trip data.";
                ModelState.AddModelError(string.Empty, "Invalid trip data.");
                return View(tripDto);
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

        public async Task<IActionResult> Details(int id)
        {
            var response = await _tripService.GetAsync(id);
            if (response?.IsSuccess == true && response.Result != null)
            {
                var trip = JsonConvert.DeserializeObject<TripDto>(Convert.ToString(response.Result));
                return View(trip);
            }

            TempData["error"] = response?.Message ?? "Trip not found.";
            ModelState.AddModelError(string.Empty, response?.Message ?? "Trip not found.");
            return View(new TripDto());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _tripService.GetAsync(id);
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
            if (tripDto == null)
            {
                TempData["error"] = "Invalid trip data.";
                ModelState.AddModelError(string.Empty, "Invalid trip data.");
                return View(tripDto);
            }

            if (!ModelState.IsValid)
            {
                TempData["error"] = "Invalid trip data.";
                ModelState.AddModelError(string.Empty, "Invalid trip data.");
                return View(tripDto);
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
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _tripService.GetAsync(id);
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _tripService.DeleteAsync(id);
            if (response?.IsSuccess == true)
            {
                TempData["success"] = "Trip deleted successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = response?.Message ?? "Failed to delete trip.";
            ModelState.AddModelError(string.Empty, response?.Message ?? "Failed to delete trip.");
            return RedirectToAction("Delete", "Trip", new { id });
        }
    }
}
