using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TripTracker.Services.ExpenseApi.Models
{
    public class Expense
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        [Required]
        [Precision(10, 2)]
        public decimal Amount { get; set; } = 0;
        public DateOnly? ExpenseDate { get; set; }
        public int ParticipantId { get; set; }
        public int TripId { get; set; }
    }
}
