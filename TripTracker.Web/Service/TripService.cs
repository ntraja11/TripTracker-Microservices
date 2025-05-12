using TripTracker.Web.Models.Dto;
using TripTracker.Web.Service.Interface;
using static TripTracker.Web.Utility.StaticDetail;

namespace TripTracker.Web.Service
{
    public class TripService : ITripService
    {
        private readonly IBaseService _baseService;
        private readonly string TripApiPath = "";
        public TripService(IBaseService baseService)
        {
            _baseService = baseService;
            TripApiPath = TripApiBasePath + "/api/trip";
        }
        public async Task<ResponseDto?> CreateAsync(TripDto tripDto)
        {
            return await _baseService.SendAsync(new Models.Dto.RequestDto
            {
                ApiType = ApiType.POST,
                Url = TripApiPath,
                Data = tripDto
            });
        }

        public async Task<ResponseDto?> DeleteAsync(int tripId)
        {
            return await _baseService.SendAsync(new Models.Dto.RequestDto
            {
                ApiType = ApiType.DELETE,
                Url = TripApiPath + "/" + tripId
            });
        }

        public async Task<ResponseDto?> GetAllAsync()
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.GET,
                Url = TripApiPath
            });
        }

        public async Task<ResponseDto?> GetAsync(int tripId)
        {
            return await _baseService.SendAsync(new Models.Dto.RequestDto
            {
                ApiType = ApiType.GET,
                Url = TripApiPath + "/" + tripId
            });
        }

        public async Task<ResponseDto?> GetByTravelGroupAsync(int travelGroupId)
        {
            return await _baseService.SendAsync(new Models.Dto.RequestDto
            {
                ApiType = ApiType.GET,
                Url = TripApiPath + "/get-by-travel-group/" + travelGroupId
            });
        }

        public async Task<ResponseDto?> UpdateAsync(TripDto tripDto)
        {
            return await _baseService.SendAsync(new Models.Dto.RequestDto
            {
                ApiType = ApiType.PUT,
                Url = TripApiPath,
                Data = tripDto
            });
        }
    }
}
