namespace TripTracker.Services.TravelGroupApi.Model
{
    public class TravelGroup
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;

        public string? Place { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;

        public DateOnly? CreatedDate { get; set; }

        public int MemberCount { get; set; } = 0;
    }
}
