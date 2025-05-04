using TripTracker.Web.Model.Dto;

namespace TripTracker.Web.Service.Interface
{
    public interface ITravelGroupService
    {
        Task<ResponseDto?> GetAllAsync();
        Task<ResponseDto?> GetAsync(int travelGroupId);

        Task<ResponseDto?> CreateAsync(TravelGroupDto dto);

        Task<ResponseDto?> UpdateAsync(TravelGroupDto dto);

        Task<ResponseDto?> DeleteAsync(int travelGroupId);
    }

}
