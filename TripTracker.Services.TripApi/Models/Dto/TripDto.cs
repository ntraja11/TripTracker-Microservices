using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TripTracker.Services.TripApi.Models.Dto
{
    public class TripDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int TravelGroupId { get; set; }
        public string? Description { get; set; }
        public string? From { get; set; }
        public string? To { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public decimal? TotalExpense { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }
        public string? CreatedByUserEmail { get; set; }

        public IEnumerable<string>? ParticipantIds { get; set; } = new List<string>();
        public IEnumerable<int>? ExpenseIds { get; set; } = new List<int>();
    }
}
