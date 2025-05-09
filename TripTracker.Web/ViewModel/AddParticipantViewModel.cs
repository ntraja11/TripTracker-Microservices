using Microsoft.AspNetCore.Mvc.Rendering;

namespace TripTracker.Web.ViewModel
{
    public class AddParticipantViewModel
    {
        public int TripId { get; set; } 
        public IEnumerable<SelectListItem> AvailableUsers { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> SelectedUsers { get; set; } = new List<SelectListItem>();
        public List<string> SelectedUserIds { get; set; } = new(); 
    }
}
