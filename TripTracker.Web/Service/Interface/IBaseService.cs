using TripTracker.Web.Models.Dto;

namespace TripTracker.Web.Service.Interface
{
    public interface IBaseService
    {
        Task<ResponseDto?> SendAsync(RequestDto requestDto);

    }
}
