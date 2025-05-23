﻿using TripTracker.Web.Models.Dto;
using TripTracker.Web.Service.Interface;
using static TripTracker.Web.Utility.StaticDetail;

namespace TripTracker.Web.Service
{
    public class TravelGroupService : ITravelGroupService
    {
        private readonly IBaseService _baseService;
        private readonly string TravelGroupApiPath = "";
        public TravelGroupService(IBaseService baseService)
        {
            _baseService = baseService;
            TravelGroupApiPath = TravelGroupApiBasePath + "/api/travel-group";
        }
        public async Task<ResponseDto?> CreateAsync(TravelGroupDto travelGroupDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.POST,
                Url = TravelGroupApiPath,
                Data = travelGroupDto
            }, withBearer: false);
        }

        public async Task<ResponseDto?> DeleteAsync(int travelGroupId)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.DELETE,
                Url = TravelGroupApiPath + "/" + travelGroupId
            });
        }

        public async Task<ResponseDto?> GetAllAsync()
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.GET,
                Url = TravelGroupApiPath
            });
        }

        public async Task<ResponseDto?> GetAsync(int travelGroupId)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.GET,
                Url = TravelGroupApiPath + "/" + travelGroupId
            });
        }

        public async Task<ResponseDto?> GetAsync(string travelGroupName)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.GET,
                Url = TravelGroupApiPath + "/get-by-name/" + travelGroupName
            }, withBearer: false);
        }

        public async Task<ResponseDto?> UpdateAsync(TravelGroupDto travelGroupDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.PUT,
                Url = TravelGroupApiPath,
                Data = travelGroupDto
            }, withBearer: false);
        }
    }
}
