using Microsoft.AspNetCore.Mvc.Rendering;
using TripTracker.Web.Models.Dto;

namespace TripTracker.Web.ViewModel
{
    public class TripDetailViewModel
    {
        public TripDto Trip { get; set; } = new TripDto();

        public decimal ParticipantShare { get; set; } = 0;
        public IEnumerable<ParticipantDto> Participants { get; set; } = new List<ParticipantDto>();

        public IEnumerable<ExpenseDto> Expenses { get; set; } = new List<ExpenseDto>();
    }
}
