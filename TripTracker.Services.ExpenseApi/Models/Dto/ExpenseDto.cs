using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TripTracker.Services.ExpenseApi.Models.Dto
{
    public class ExpenseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string? Description { get; set; } = String.Empty;
        public decimal Amount { get; set; } = 0;
        public DateOnly? ExpenseDate { get; set; }
        public int ParticipantId { get; set; }
        public int TripId { get; set; }
    }
}
