using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TripTracker.Services.TravelGroupApi.Data;
using TripTracker.Services.TravelGroupApi.Model;
using TripTracker.Services.TravelGroupApi.Model.Dto;

namespace TripTracker.Services.TravelGroupApi.Controllers
{
    [Route("api/group")]
    [ApiController]
    public class TravelGroupApiController : ControllerBase
    {
        private readonly TravelGroupDbContext _db;
        private readonly ResponseDto _responseDto;

        public TravelGroupApiController(TravelGroupDbContext db)
        {
            _db = db;
            _responseDto = new ResponseDto();
        }

        [HttpGet]
        public ResponseDto GetAll()
        {
            try
            {
                _responseDto.Result = _db.TravelGroups.ToList();                
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message.ToString();
            }
            return _responseDto;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
                _responseDto.Result = _db.TravelGroups.First(g => g.Id == id);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message.ToString();
            }
            return _responseDto;
        }
    }
}
