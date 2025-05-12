using TripTracker.Services.AuthApi.Models.Dto;

namespace TripTracker.Services.AuthApi.Service.Interface
{
    public interface ITravelGroupApiService
    {
        Task<ResponseDto?> CreateAsync(TravelGroupDto dto);
        Task<TravelGroupDto> GetTravelGroupByName(string travelGroupName);
    }
}
