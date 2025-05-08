using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripTracker.Services.ParticipantApi.Models
{
    public class Participant
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = String.Empty;
        [Required]
        public string Email { get; set; } = String.Empty;
        public int TripId { get; set; }
        [NotMapped]
        [Precision(10, 2)]
        public decimal TotalTripExpense { get; set; } = 0;
    }
}
