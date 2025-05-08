using AutoMapper;
using TripTracker.Services.ParticipantApi.Models;
using TripTracker.Services.ParticipantApi.Models.Dto;

namespace TripTracker.Services.ParticipantApi
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Participant, ParticipantDto>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
