using AutoMapper;
using TripTracker.Services.TravelGroupApi.Models;
using TripTracker.Services.TravelGroupApi.Models.Dto;

namespace TripTracker.Services.TravelGroupApi
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<TravelGroup, TravelGroupDto>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
