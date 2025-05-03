using AutoMapper;
using TripTracker.Services.TravelGroupApi.Model;
using TripTracker.Services.TravelGroupApi.Model.Dto;

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
