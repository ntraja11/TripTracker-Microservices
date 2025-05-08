using TripTracker.Web.Models.Dto;

namespace TripTracker.Web.Service.Interface
{
    public interface IParticipantService
    {
        Task<ResponseDto?> GetAllAsync();
        Task<ResponseDto?> GetAsync(int participantId);
        Task<ResponseDto?> CreateAsync(ParticipantDto dto);
        Task<ResponseDto?> UpdateAsync(ParticipantDto dto);
        Task<ResponseDto?> DeleteAsync(int participantId);
    }

}
