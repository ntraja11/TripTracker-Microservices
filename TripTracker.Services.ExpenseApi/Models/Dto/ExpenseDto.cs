using Microsoft.AspNetCore.Mvc.Rendering;

namespace TripTracker.Services.ExpenseApi.Models.Dto
{
    public class ExpenseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string? Description { get; set; } = String.Empty;
        public decimal Amount { get; set; } = 0;
        public DateOnly? Date { get; set; }
        public string? ParticipantName { get; set; }
        public int ParticipantId { get; set; }
        public int TripId { get; set; }
    }
}
