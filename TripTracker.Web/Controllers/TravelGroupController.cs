using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TripTracker.Web.Model.Dto;
using TripTracker.Web.Service.Interface;

namespace TripTracker.Web.Controllers
{
    public class TravelGroupController : Controller
    {
        private readonly ITravelGroupService _travelGroupService;

        public TravelGroupController(ITravelGroupService travelGroupService)
        {
            _travelGroupService = travelGroupService;
        }
       
        public async Task<IActionResult> Index()
        {
            List<TravelGroupDto> travelGroups = new();

            var response = await _travelGroupService.GetAllAsync();

            if (response?.IsSuccess == true && response.Result != null)
            {
                try
                {
                    travelGroups = JsonConvert.DeserializeObject<List<TravelGroupDto>>(Convert.ToString(response.Result)) ?? new List<TravelGroupDto>();
                }
                catch (JsonException ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error parsing data: {ex.Message}");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, response?.Message ?? "Failed to retrieve travel groups.");
            }

            return View(travelGroups);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TravelGroupDto travelGroupDto)
        {
            if (travelGroupDto == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid travel group data.");
                return View(travelGroupDto);
            }

            var response = await _travelGroupService.CreateAsync(travelGroupDto);
            if (response?.IsSuccess == true)
            {
                return RedirectToAction(nameof(Index)); 
            }

            ModelState.AddModelError(string.Empty, response?.Message ?? "Failed to create travel group.");
            return View(travelGroupDto);
        }

        public async Task<IActionResult> Details(int id)
        {
            var response = await _travelGroupService.GetAsync(id);
            if (response?.IsSuccess == true && response.Result != null)
            {
                var travelGroup = JsonConvert.DeserializeObject<TravelGroupDto>(Convert.ToString(response.Result));
                return View(travelGroup);
            }

            ModelState.AddModelError(string.Empty, response?.Message ?? "Travel group not found.");
            return View(new TravelGroupDto()); // Return an empty model if not found
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _travelGroupService.GetAsync(id);
            if (response?.IsSuccess == true && response.Result != null)
            {
                var travelGroup = JsonConvert.DeserializeObject<TravelGroupDto>(Convert.ToString(response.Result));
                return View(travelGroup);
            }

            ModelState.AddModelError(string.Empty, response?.Message ?? "Travel group not found.");
            return View(new TravelGroupDto());
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TravelGroupDto travelGroupDto)
        {
            if (travelGroupDto == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid travel group data.");
                return View(travelGroupDto);
            }

            var response = await _travelGroupService.UpdateAsync(travelGroupDto);
            if (response?.IsSuccess == true)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, response?.Message ?? "Failed to update travel group.");
            return View(travelGroupDto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _travelGroupService.GetAsync(id);
            if (response?.IsSuccess == true && response.Result != null)
            {
                var travelGroup = JsonConvert.DeserializeObject<TravelGroupDto>(Convert.ToString(response.Result));
                return View(travelGroup);
            }

            ModelState.AddModelError(string.Empty, response?.Message ?? "Travel group not found.");
            return View(new TravelGroupDto());
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _travelGroupService.DeleteAsync(id);
            if (response?.IsSuccess == true)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, response?.Message ?? "Failed to delete travel group.");
            return RedirectToAction("Delete", "TravelGroup", new {id});
        }
    }
}
