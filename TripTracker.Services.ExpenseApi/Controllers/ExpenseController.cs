using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripTracker.Services.ExpenseApi.Data;
using TripTracker.Services.ExpenseApi.Models;
using TripTracker.Services.ExpenseApi.Models.Dto;

namespace TripTracker.Services.ExpenseApi.Controllers
{
    [Route("api/expense")]
    [ApiController]
    [Authorize]
    public class ExpenseApiController : ControllerBase
    {
        private readonly ExpenseDbContext _db;
        private readonly ResponseDto _responseDto;
        private readonly IMapper _mapper;

        public ExpenseApiController(ExpenseDbContext db, IMapper mapper, ResponseDto responseDto)
        {
            _db = db;
            _mapper = mapper;
            _responseDto = responseDto;
        }

        [HttpGet]
        [Route("get-all-by-trip/{tripId:int}")]
        public async Task<ResponseDto> GetAllByTrip(int tripId)
        {
            try
            {
                _responseDto.Result = _mapper.Map<IEnumerable<ExpenseDto>>(
                    await _db.Expenses.AsNoTracking().Where(e => e.TripId == tripId).ToListAsync());
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
            var expense = await _db.Expenses.FindAsync(id);

            if (expense == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Expense not found.";
                return _responseDto;
            }

            try
            {
                _responseDto.Result = _mapper.Map<ExpenseDto>(expense);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ResponseDto> Post([FromBody] ExpenseDto expenseDto)
        {
            try
            {
                if (expenseDto == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Expense is null.";
                    return _responseDto;
                }
                
                Expense expense = _mapper.Map<Expense>(expenseDto);
                await _db.Expenses.AddAsync(expense);
                await _db.SaveChangesAsync();
                _responseDto.Result = _mapper.Map<ExpenseDto>(expense);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public async Task<ResponseDto> Put([FromBody] ExpenseDto expenseDto)
        {
            try
            {
                if (expenseDto == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Expense is null.";
                    return _responseDto;
                }

                Expense expense = _mapper.Map<Expense>(expenseDto);
                _db.Expenses.Update(expense);
                await _db.SaveChangesAsync();
                _responseDto.Result = _mapper.Map<ExpenseDto>(expense);
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
        [Authorize(Roles = "ADMIN")]
        public async Task<ResponseDto> Delete(int id)
        {
            try
            {
                var expense = await _db.Expenses.FindAsync(id);
                if (expense == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Expense not found.";
                    return _responseDto;
                }

                _db.Expenses.Remove(expense);
                await _db.SaveChangesAsync();
                _responseDto.Message = "Expense deleted successfully.";
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
