using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TripTracker.Web.Models.Dto;
using TripTracker.Web.Service.Interface;

namespace TripTracker.Web.Controllers
{
    [Authorize]
    public class ExpenseController : Controller
    {
        private readonly IExpenseService _expenseService;

        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        public async Task<IActionResult> Index()
        {
            List<ExpenseDto> expenses = new();

            var response = await _expenseService.GetAllAsync();

            if (response?.IsSuccess == true && response.Result != null)
            {
                try
                {
                    expenses = JsonConvert
                        .DeserializeObject<List<ExpenseDto>>(Convert.ToString(response.Result))
                        ?? new List<ExpenseDto>();
                }
                catch (JsonException ex)
                {
                    TempData["error"] = $"Error parsing data: {ex.Message}";
                    ModelState.AddModelError(string.Empty, $"Error parsing data: {ex.Message}");
                }
            }
            else
            {
                TempData["error"] = response?.Message ?? "Failed to retrieve expenses.";
                ModelState.AddModelError(string.Empty, response?.Message ?? "Failed to retrieve expenses.");
            }

            return View(expenses);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExpenseDto expenseDto)
        {
            if (expenseDto == null)
            {
                TempData["error"] = "Invalid expense data.";
                ModelState.AddModelError(string.Empty, "Invalid expense data.");
                return View(expenseDto);
            }

            if (!ModelState.IsValid)
            {
                TempData["error"] = "Invalid expense data.";
                ModelState.AddModelError(string.Empty, "Invalid expense data.");
                return View(expenseDto);
            }

            var response = await _expenseService.CreateAsync(expenseDto);
            if (response?.IsSuccess == true)
            {
                TempData["success"] = "Expense created successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = response?.Message ?? "Failed to create expense.";
            ModelState.AddModelError(string.Empty, response?.Message ?? "Failed to create expense.");
            return View(expenseDto);
        }

        public async Task<IActionResult> Details(int id)
        {
            var response = await _expenseService.GetAsync(id);
            if (response?.IsSuccess == true && response.Result != null)
            {
                var expense = JsonConvert.DeserializeObject<ExpenseDto>(Convert.ToString(response.Result));
                return View(expense);
            }

            TempData["error"] = response?.Message ?? "Expense not found.";
            ModelState.AddModelError(string.Empty, response?.Message ?? "Expense not found.");
            return View(new ExpenseDto());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _expenseService.GetAsync(id);
            if (response != null && response?.IsSuccess == true && response.Result != null)
            {
                var expense = JsonConvert.DeserializeObject<ExpenseDto>(Convert.ToString(response.Result));
                return View(expense);
            }

            TempData["error"] = response?.Message ?? "Expense not found.";
            ModelState.AddModelError(string.Empty, response?.Message ?? "Expense not found.");
            return View(new ExpenseDto());
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ExpenseDto expenseDto)
        {
            if (expenseDto == null)
            {
                TempData["error"] = "Invalid expense data.";
                ModelState.AddModelError(string.Empty, "Invalid expense data.");
                return View(expenseDto);
            }

            if (!ModelState.IsValid)
            {
                TempData["error"] = "Invalid expense data.";
                ModelState.AddModelError(string.Empty, "Invalid expense data.");
                return View(expenseDto);
            }

            var response = await _expenseService.UpdateAsync(expenseDto);
            if (response?.IsSuccess == true)
            {
                TempData["success"] = "Expense updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = response?.Message ?? "Failed to update expense.";
            ModelState.AddModelError(string.Empty, response?.Message ?? "Failed to update expense.");
            return View(expenseDto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _expenseService.GetAsync(id);
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _expenseService.DeleteAsync(id);
            if (response?.IsSuccess == true)
            {
                TempData["success"] = "Expense deleted successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = response?.Message ?? "Failed to delete expense.";
            ModelState.AddModelError(string.Empty, response?.Message ?? "Failed to delete expense.");
            return RedirectToAction("Delete", "Expense", new { id });
        }
    }
}
