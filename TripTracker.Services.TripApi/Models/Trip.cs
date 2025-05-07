using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TripTracker.Services.TripApi.Models
{
    public class Trip
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public int TravelGroupId { get; set; } = 0;
        public string? Description { get; set; }
        [Required]
        public string? From { get; set; }
        [Required]
        public string? To { get; set; }

        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }

        [Display(Name = "Total Expense")]
        [Precision(10, 2)]
        public decimal? TotalExpense { get; set; }
                
        public string? Status { get; set; }

        public string? Notes { get; set; }

        public string? CreatedByUserEmail { get; set; }

        public IEnumerable<string>? ParticipantIds { get; set; } = new List<string>();
        public IEnumerable<int>? ExpenseIds { get; set; } = new List<int>();


    }
}
