using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using TripTracker.Web.Models.Dto;
using TripTracker.Web.Service;
using TripTracker.Web.Service.Interface;
using TripTracker.Web.ViewModel;

namespace TripTracker.Web.Controllers
{
    [Authorize]
    public class ExpenseController : Controller
    {
        private readonly IExpenseService _expenseService;
        private readonly IParticipantService _participantService;

        public ExpenseController(IExpenseService expenseService, IParticipantService participantService)
        {
            _expenseService = expenseService;
            _participantService = participantService;
        }
        
        [HttpGet]
        public async Task<IActionResult> Create(int tripId)
        {
            if (tripId == 0)
            {
                TempData["error"] = "Invalid trip ID.";
                ModelState.AddModelError(string.Empty, "Invalid trip ID.");
                return RedirectToAction("Details", "Trips", new { tripId = tripId });
            }

            
            UpsertExpenseViewModel upsertExpenseViewModel = await GetUpsertExpenseViewModel(tripId);
            upsertExpenseViewModel.Expense = new ExpenseDto
            {
                TripId = tripId
            };

            return View(upsertExpenseViewModel);
        }

        private async Task<UpsertExpenseViewModel> GetUpsertExpenseViewModel(int tripId)
        {
            var participantsResponse = await _participantService.GetAllByTripAsync(tripId);

            var participants = (participantsResponse!.IsSuccess == true && participantsResponse != null) ?
                JsonConvert.DeserializeObject<List<ParticipantDto>>(Convert.ToString(participantsResponse.Result)) : new List<ParticipantDto>();

            return new UpsertExpenseViewModel
            {                
                Participants = participants!.Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                }).ToList()
            };
        }

        [HttpPost]
        public async Task<IActionResult> Create(UpsertExpenseViewModel upsertExpenseViewModel)
        {
            if (upsertExpenseViewModel.Expense == null || !ModelState.IsValid)
            {
                TempData["error"] = "Invalid expense data.";
                ModelState.AddModelError(string.Empty, "Invalid expense data.");
                return View(await GetUpsertExpenseViewModel(upsertExpenseViewModel.Expense!.TripId));
            }
                        
            upsertExpenseViewModel.Expense.ParticipantName = await GetParticipantName(upsertExpenseViewModel.Expense.ParticipantId);

            var response = await _expenseService.CreateAsync(upsertExpenseViewModel.Expense);
            if (response?.IsSuccess == true)
            {
                TempData["success"] = "Expense created successfully.";
                return RedirectToAction("Details", "Trips", new { tripId = upsertExpenseViewModel.Expense.TripId});
            }

            TempData["error"] = response?.Message ?? "Failed to create expense.";
            ModelState.AddModelError(string.Empty, response?.Message ?? "Failed to create expense.");

            UpsertExpenseViewModel freshUpsertExpenseViewModel = await GetUpsertExpenseViewModel(upsertExpenseViewModel.Expense.TripId);
            freshUpsertExpenseViewModel.Expense = new ExpenseDto
            {
                TripId = upsertExpenseViewModel.Expense.TripId
            };

            return View(freshUpsertExpenseViewModel);
        }

        private async Task<string> GetParticipantName(int participantId)
        {
            var participantResponse = await _participantService.GetAsync(participantId);
            var participant = (participantResponse!.IsSuccess == true && participantResponse != null) ?
                JsonConvert.DeserializeObject<ParticipantDto>(Convert.ToString(participantResponse.Result)) : new ParticipantDto();
            return participant!.Name!;
        }

        public async Task<IActionResult> Details(int expenseId)
        {
            var response = await _expenseService.GetAsync(expenseId);
            if (response?.IsSuccess == true && response.Result != null)
            {
                var expense = JsonConvert.DeserializeObject<ExpenseDto>(Convert.ToString(response.Result));
                return View(expense);
            }

            TempData["error"] = response?.Message ?? "Expense not found.";
            ModelState.AddModelError(string.Empty, response?.Message ?? "Expense not found.");
            return View(new ExpenseDto());
        }

        private async Task<ExpenseDto> GetExpense(int expenseId)
        {
            var response = await _expenseService.GetAsync(expenseId);
            var expense =  (response?.IsSuccess == true && response.Result != null) ?
                JsonConvert.DeserializeObject<ExpenseDto>(Convert.ToString(response.Result))
                : new ExpenseDto();

            return expense!;


        }

        [HttpGet]
        public async Task<IActionResult> Edit(int tripId, int expenseId)
        {
            UpsertExpenseViewModel upsertExpenseViewModel = await GetUpsertExpenseViewModel(tripId);
            upsertExpenseViewModel.Expense = await GetExpense(expenseId);

            return View(upsertExpenseViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpsertExpenseViewModel upsertExpenseViewModel)
        {
            if (upsertExpenseViewModel.Expense == null || !ModelState.IsValid)
            {
                TempData["error"] = "Invalid expense data.";
                ModelState.AddModelError(string.Empty, "Invalid expense data.");
                return View(await GetUpsertExpenseViewModel(upsertExpenseViewModel.Expense!.TripId));
            }

            upsertExpenseViewModel.Expense.ParticipantName = await GetParticipantName(upsertExpenseViewModel.Expense.ParticipantId);

            var response = await _expenseService.UpdateAsync(upsertExpenseViewModel.Expense);
            if (response?.IsSuccess == true)
            {
                TempData["success"] = "Expense updated successfully.";
                return RedirectToAction("Details", "Expense", new { expenseId = upsertExpenseViewModel.Expense.Id});
            }

            TempData["error"] = response?.Message ?? "Failed to update expense.";
            ModelState.AddModelError(string.Empty, response?.Message ?? "Failed to update expense.");

            UpsertExpenseViewModel freshUpsertExpenseViewModel = await GetUpsertExpenseViewModel(upsertExpenseViewModel.Expense.TripId);
            freshUpsertExpenseViewModel.Expense = await GetExpense(upsertExpenseViewModel.Expense.Id);
            
            return View(freshUpsertExpenseViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int expenseId)
        {
            var response = await _expenseService.GetAsync(expenseId);
            if (response?.IsSuccess == true && response.Result != null)
            {
                var expense = JsonConvert.DeserializeObject<ExpenseDto>(Convert.ToString(response.Result));
                return View(expense);
            }

            TempData["error"] = response?.Message ?? "Expense not found.";
            ModelState.AddModelError(string.Empty, response?.Message ?? "Expense not found.");
            return View(new ExpenseDto());
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int expenseId)
        {
            var response = await _expenseService.DeleteAsync(expenseId);
            if (response?.IsSuccess == true)
            {
                TempData["success"] = "Expense deleted successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = response?.Message ?? "Failed to delete expense.";
            ModelState.AddModelError(string.Empty, response?.Message ?? "Failed to delete expense.");
            return RedirectToAction("Delete", "Expense", new { expenseId });
        }
    }
}
