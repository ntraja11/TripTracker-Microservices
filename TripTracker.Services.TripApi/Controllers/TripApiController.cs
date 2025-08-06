using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripTracker.MessageBus;
using TripTracker.Services.TripApi.Data;
using TripTracker.Services.TripApi.Models;
using TripTracker.Services.TripApi.Models.Dto;

namespace TripTracker.Services.TripApi.Controllers
{
    [Route("api/trip")]
    [ApiController]
    [Authorize]
    public class TripApiController : ControllerBase
    {
        private readonly TripDbContext _db;
        private readonly ResponseDto _responseDto;
        private readonly IMapper _mapper;
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;

        public TripApiController(TripDbContext db, IMapper mapper, ResponseDto responseDto, IMessageBus messageBus, IConfiguration configuration)
        {
            _db = db;
            _mapper = mapper;
            _responseDto = responseDto;
            _messageBus = messageBus;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ResponseDto> Get()
        {
            try
            {
                _responseDto.Result = _mapper.Map<IEnumerable<TripDto>>(
                    await _db.Trips.AsNoTracking().ToListAsync());
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
            var trip = await _db.Trips.FindAsync(id);

            if (trip == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Trip group not found.";
                return _responseDto;
            }

            try
            {
                _responseDto.Result = _mapper.Map<TripDto>(trip);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpGet]
        [Route("GetByTravelGroup/{travelGroupId:int}")]
        public async Task<ResponseDto> GetByTravelGroup(int travelGroupId)
        { 
            try
            {
                var trips = await _db.Trips.Where(t => t.TravelGroupId == travelGroupId)
                .AsNoTracking().ToListAsync();

                if (trips.Count == 0)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "No trips created in this travel group";
                    return _responseDto;
                }
                _responseDto.Result = _mapper.Map<IEnumerable<TripDto>>(trips);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPost("EmailTripCreated")]
        public async Task<ResponseDto> EmailTripCreated([FromBody] TripDto tripDto)
        {
            try
            {               
                await _messageBus.PublishMessage(tripDto, 
                    _configuration.GetValue<string>("QueueNames:EmailTripCreated"),
                    _configuration.GetValue<string>("ConnectionStrings:ServiceBusConnection"));
                _responseDto.Result = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPost]
        //[Authorize(Roles = "ADMIN")]
        public async Task<ResponseDto> Post([FromBody] TripDto tripDto)
        {
            try
            {
                if (tripDto == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Trip is null.";
                    return _responseDto;
                }

                if (await _db.Trips.AnyAsync(tg => tg.Name!.ToLower() == tripDto.Name!.ToLower()))
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "A Trip with this name already exists";
                    return _responseDto;
                }

                Trip trip = _mapper.Map<Trip>(tripDto);
                await _db.Trips.AddAsync(trip);
                await _db.SaveChangesAsync();
                _responseDto.Result = _mapper.Map<TripDto>(trip);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPut]
        //[Authorize(Roles = "ADMIN")]
        public async Task<ResponseDto> Put([FromBody] TripDto tripDto)
        {
            try
            {
                if (tripDto == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Trip group is null.";
                    return _responseDto;
                }

                Trip trip = _mapper.Map<Trip>(tripDto);
                _db.Trips.Update(trip);
                await _db.SaveChangesAsync();
                _responseDto.Result = _mapper.Map<TripDto>(trip);
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
        //[Authorize(Roles = "ADMIN")]
        public async Task<ResponseDto> Delete(int id)
        {
            try
            {
                var trip = await _db.Trips.FindAsync(id);
                if (trip == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Trip group not found.";
                    return _responseDto;
                }

                _db.Trips.Remove(trip);
                await _db.SaveChangesAsync();
                _responseDto.Message = "Trip group deleted successfully.";
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
