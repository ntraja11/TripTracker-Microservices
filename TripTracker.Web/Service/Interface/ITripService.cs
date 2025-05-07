using TripTracker.Web.Models.Dto;

namespace TripTracker.Web.Service.Interface
{
    public interface ITripService
    {
        Task<ResponseDto?> GetAllAsync();
        Task<ResponseDto?> GetAsync(int tripId);

        Task<ResponseDto?> GetByTravelGroupAsync(int travelGroupId);

        Task<ResponseDto?> CreateAsync(TripDto dto);

        Task<ResponseDto?> UpdateAsync(TripDto dto);

        Task<ResponseDto?> DeleteAsync(int tripId);
    }

}
