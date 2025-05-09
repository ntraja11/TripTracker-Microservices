using AutoMapper;
using TripTracker.Services.AuthApi.Models;
using TripTracker.Services.AuthApi.Models.Dto;

namespace TripTracker.Services.AuthApi
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ApplicationUser, UserDto>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
