using TripTracker.Web.Models.Dto;

namespace TripTracker.Web.Service.Interface
{
    public interface IExpenseService
    {
        Task<ResponseDto?> GetAllAsync();
        Task<ResponseDto?> GetAsync(int expenseId);
        Task<ResponseDto?> CreateAsync(ExpenseDto dto);
        Task<ResponseDto?> UpdateAsync(ExpenseDto dto);
        Task<ResponseDto?> DeleteAsync(int expenseId);
    }

}
