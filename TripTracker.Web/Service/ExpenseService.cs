using TripTracker.Web.Models.Dto;
using TripTracker.Web.Service.Interface;
using static TripTracker.Web.Utility.StaticDetail;

namespace TripTracker.Web.Service
{
    public class ExpenseService : IExpenseService
    {
        private readonly IBaseService _baseService;
        private readonly string ExpenseApiPath = "";
        public ExpenseService(IBaseService baseService)
        {
            _baseService = baseService;
            ExpenseApiPath = ExpenseApiBasePath + "/api/expense/";
        }
        public async Task<ResponseDto?> CreateAsync(ExpenseDto expenseDto)
        {
            return await _baseService.SendAsync(new Models.Dto.RequestDto
            {
                ApiType = ApiType.POST,
                Url = ExpenseApiPath,
                Data = expenseDto
            });
        }

        public async Task<ResponseDto?> DeleteAsync(int expenseId)
        {
            return await _baseService.SendAsync(new Models.Dto.RequestDto
            {
                ApiType = ApiType.DELETE,
                Url = ExpenseApiPath + expenseId
            });
        }


        public async Task<ResponseDto?> GetAllByTripAsync(int tripId)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.GET,
                Url = ExpenseApiPath + "get-all-by-trip/" + tripId
            });
        }

        public async Task<ResponseDto?> GetAsync(int expenseId)
        {
            return await _baseService.SendAsync(new Models.Dto.RequestDto
            {
                ApiType = ApiType.GET,
                Url = ExpenseApiPath + expenseId
            });
        }

       
        public async Task<ResponseDto?> UpdateAsync(ExpenseDto expenseDto)
        {
            return await _baseService.SendAsync(new Models.Dto.RequestDto
            {
                ApiType = ApiType.PUT,
                Url = ExpenseApiPath,
                Data = expenseDto
            });
        }
    }
}
