using TripTracker.Web.Models.Dto;
using TripTracker.Web.Service.Interface;
using static TripTracker.Web.Utility.StaticDetail;

namespace TripTracker.Web.Service
{
    public class ParticipantService : IParticipantService
    {
        private readonly IBaseService _baseService;
        private readonly string ParticipantApiPath = "";
        public ParticipantService(IBaseService baseService)
        {
            _baseService = baseService;
            ParticipantApiPath = ParticipantApiBasePath + "/api/participant/";
        }
        public async Task<ResponseDto?> CreateAsync(ParticipantDto participantDto)
        {
            return await _baseService.SendAsync(new Models.Dto.RequestDto
            {
                ApiType = ApiType.POST,
                Url = ParticipantApiPath,
                Data = participantDto
            });
        }

        public async Task<ResponseDto?> DeleteAsync(int participantId)
        {
            return await _baseService.SendAsync(new Models.Dto.RequestDto
            {
                ApiType = ApiType.DELETE,
                Url = ParticipantApiPath + participantId
            });
        }

        public async Task<ResponseDto?> GetAllAsync()
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.GET,
                Url = ParticipantApiPath
            });
        }

        public async Task<ResponseDto?> GetAsync(int participantId)
        {
            return await _baseService.SendAsync(new Models.Dto.RequestDto
            {
                ApiType = ApiType.GET,
                Url = ParticipantApiPath + participantId
            });
        }

       
        public async Task<ResponseDto?> UpdateAsync(ParticipantDto participantDto)
        {
            return await _baseService.SendAsync(new Models.Dto.RequestDto
            {
                ApiType = ApiType.PUT,
                Url = ParticipantApiPath,
                Data = participantDto
            });
        }
    }
}
