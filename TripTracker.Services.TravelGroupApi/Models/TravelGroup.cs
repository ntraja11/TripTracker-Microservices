using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TripTracker.Services.TravelGroupApi.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class TravelGroup
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; } = string.Empty;

        public string? Place { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;

        public DateOnly? CreatedDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        public int MemberCount { get; set; } = 0;
    }
}
