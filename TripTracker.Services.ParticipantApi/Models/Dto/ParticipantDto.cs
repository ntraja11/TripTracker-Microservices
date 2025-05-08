namespace TripTracker.Services.ParticipantApi.Models.Dto
{
    public class ParticipantDto
    {
        public int? Id { get; set; }
        public string? Name { get; set; } = String.Empty;
        public string? Email { get; set; } = String.Empty;
        public int? TripId { get; set; }
        public decimal? TotalTripExpense { get; set; } = 0;
    }
}
