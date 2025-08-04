namespace TripTracker.Services.EmailApi.Models
{
    public class EmailLogger
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }

        public DateOnly EmailSentOn { get; set; }
    }
}
