using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripTracker.Services.TravelGroupApi.Data;
using TripTracker.Services.TravelGroupApi.Models;
using TripTracker.Services.TravelGroupApi.Models.Dto;

namespace TripTracker.Services.TravelGroupApi.Controllers
{
    [Route("api/travel-group")]
    [ApiController]
    public class TravelGroupApiController : ControllerBase
    {
        private readonly TravelGroupDbContext _db;
        private readonly ResponseDto _responseDto;
        private readonly IMapper _mapper;

        public TravelGroupApiController(TravelGroupDbContext db, IMapper mapper, ResponseDto responseDto)
        {
            _db = db;
            _mapper = mapper;
            _responseDto = responseDto;
        }

        [HttpGet]
        public async Task<ResponseDto> GetAll()
        {
            try
            {
                _responseDto.Result = _mapper.Map<IEnumerable<TravelGroupDto>>(
                    await _db.TravelGroups.AsNoTracking().ToListAsync());
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ResponseDto> Get(int id)
        {
            var travelGroup = await _db.TravelGroups.FindAsync(id);

            if (travelGroup == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Travel group not found.";
                return _responseDto;
            }

            try
            {
                _responseDto.Result = _mapper.Map<TravelGroupDto>(travelGroup);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpGet]
        [Route("getByName/{travelGroupName}")]
        public async Task<ResponseDto> Get(string travelGroupName)
        {
            var travelGroup = await _db.TravelGroups.FirstOrDefaultAsync(tg => tg.Name == travelGroupName);

            if (travelGroup == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Travel group not found.";
                return _responseDto;
            }

            try
            {
                _responseDto.Result = _mapper.Map<TravelGroupDto>(travelGroup);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPost]
        public async Task<ResponseDto> Post([FromBody] TravelGroupDto travelGroupDto)
        {
            try
            {
                if (travelGroupDto == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Travel group is null.";
                    return _responseDto;
                }

                if (await _db.TravelGroups.AnyAsync(tg => tg.Name!.ToLower() == travelGroupDto.Name!.ToLower()))
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "A TravelGroup with this name already exists";
                    return _responseDto;
                }

                TravelGroup travelGroup = _mapper.Map<TravelGroup>(travelGroupDto);
                await _db.TravelGroups.AddAsync(travelGroup);
                await _db.SaveChangesAsync();
                _responseDto.Result = _mapper.Map<TravelGroupDto>(travelGroup);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPut]
        public async Task<ResponseDto> Put([FromBody] TravelGroupDto travelGroupDto)
        {
            try
            {
                if (travelGroupDto == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Travel group is null.";
                    return _responseDto;
                }

                TravelGroup travelGroup = _mapper.Map<TravelGroup>(travelGroupDto);
                _db.TravelGroups.Update(travelGroup);
                await _db.SaveChangesAsync();
                _responseDto.Result = _mapper.Map<TravelGroupDto>(travelGroup);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ResponseDto> Delete(int id)
        {
            try
            {
                var travelGroup = await _db.TravelGroups.FindAsync(id);
                if (travelGroup == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Travel group not found.";
                    return _responseDto;
                }

                _db.TravelGroups.Remove(travelGroup);
                await _db.SaveChangesAsync();
                _responseDto.Message = "Travel group deleted successfully.";
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }
    }
}