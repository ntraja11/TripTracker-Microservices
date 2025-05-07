using AutoMapper;
using TripTracker.Services.TripApi.Models;
using TripTracker.Services.TripApi.Models.Dto;

namespace TripTracker.Services.TripApi
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Trip, TripDto>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
