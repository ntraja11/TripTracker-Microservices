namespace TripTracker.Services.AuthApi.Models.Dto
{
    public class TravelGroupDto
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;

        public string? Place { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;

        public int MemberCount { get; set; } = 0;
    }
}
