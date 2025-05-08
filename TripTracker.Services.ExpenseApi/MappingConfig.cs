using AutoMapper;
using TripTracker.Services.ExpenseApi.Models;
using TripTracker.Services.ExpenseApi.Models.Dto;

namespace TripTracker.Services.ExpenseApi
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Expense, ExpenseDto>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
