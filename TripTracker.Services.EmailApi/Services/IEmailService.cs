using TripTracker.Services.EmailApi.Models.Dto;

namespace TripTracker.Services.EmailApi.Services
{
    public interface IEmailService
    {
        void SendTripCreatedEmail(TripDto tripDto);
    }
}
