using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripTracker.Services.ParticipantApi.Data;
using TripTracker.Services.ParticipantApi.Models;
using TripTracker.Services.ParticipantApi.Models.Dto;

namespace TripTracker.Services.ParticipantApi.Controllers
{
    [Route("api/participant")]
    [ApiController]
    [Authorize]
    public class ParticipantApiController : ControllerBase

    {
        private readonly ParticipantDbContext _db;
        private readonly ResponseDto _responseDto;
        private readonly IMapper _mapper;

        public ParticipantApiController(ParticipantDbContext db, IMapper mapper, ResponseDto responseDto)
        {
            _db = db;
            _mapper = mapper;
            _responseDto = responseDto;
        }
                

        [HttpGet]
        [Route("GetAllByTrip/{tripId}")]
        public async Task<ResponseDto> GetAllByTrip(int tripId)
        {
            try
            {
                _responseDto.Result = _mapper.Map<IEnumerable<ParticipantDto>>(
                    await _db.Participants.AsNoTracking().Where(p => p.TripId == tripId).ToListAsync());
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
            var participant = await _db.Participants.FindAsync(id);

            if (participant == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Participant not found.";
                return _responseDto;
            }

            try
            {
                _responseDto.Result = _mapper.Map<ParticipantDto>(participant);
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
        public async Task<ResponseDto> Post([FromBody] ParticipantDto participantDto)
        {
            try
            {
                if (participantDto == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Participant group is null.";
                    return _responseDto;
                }

                if (await _db.Participants.AnyAsync(tg =>
                        tg.Name!.ToLower() == participantDto.Name!.ToLower()
                        && tg.TripId == participantDto.TripId))
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "A Participant with this name already exists";
                    return _responseDto;
                }

                Participant participant = _mapper.Map<Participant>(participantDto);
                await _db.Participants.AddAsync(participant);
                await _db.SaveChangesAsync();
                _responseDto.Result = _mapper.Map<ParticipantDto>(participant);
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
        public async Task<ResponseDto> Put([FromBody] ParticipantDto participantDto)
        {
            try
            {
                if (participantDto == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Participant is null.";
                    return _responseDto;
                }

                Participant participant = _mapper.Map<Participant>(participantDto);
                _db.Participants.Update(participant);
                await _db.SaveChangesAsync();
                _responseDto.Result = _mapper.Map<ParticipantDto>(participant);
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
                var participant = await _db.Participants.FindAsync(id);
                if (participant == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Participant not found.";
                    return _responseDto;
                }

                _db.Participants.Remove(participant);
                await _db.SaveChangesAsync();
                _responseDto.Message = "Participant deleted successfully.";
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
