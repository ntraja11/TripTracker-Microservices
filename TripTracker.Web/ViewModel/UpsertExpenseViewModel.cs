using Microsoft.AspNetCore.Mvc.Rendering;
using TripTracker.Web.Models.Dto;

namespace TripTracker.Web.ViewModel
{
    public class UpsertExpenseViewModel
    {
        public ExpenseDto Expense { get; set; } = new ExpenseDto();

        public IEnumerable<SelectListItem> Participants { get; set; } = new List<SelectListItem>();
    }
}
