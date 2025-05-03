namespace TripTracker.Services.TravelGroupApi.Model.Dto
{
    public class TravelGroupDto
    {
        public string? Name { get; set; } = string.Empty;

        public string? Place { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;

        public int MemberCount { get; set; } = 0;
    }
}
