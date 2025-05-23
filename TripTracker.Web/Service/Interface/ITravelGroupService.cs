﻿using TripTracker.Web.Models.Dto;

namespace TripTracker.Web.Service.Interface
{
    public interface ITravelGroupService
    {
        Task<ResponseDto?> GetAllAsync();
        Task<ResponseDto?> GetAsync(int travelGroupId);

        Task<ResponseDto?> GetAsync(string travelGroupName);

        Task<ResponseDto?> CreateAsync(TravelGroupDto dto);

        Task<ResponseDto?> UpdateAsync(TravelGroupDto dto);

        Task<ResponseDto?> DeleteAsync(int travelGroupId);
    }

}
