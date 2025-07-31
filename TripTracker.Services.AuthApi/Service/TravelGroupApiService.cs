using TripTracker.Services.AuthApi.Models.Dto;
using TripTracker.Services.AuthApi.Service.Interface;
using Newtonsoft.Json;
using System.Text;

namespace TripTracker.Services.AuthApi.Service
{
    public class TravelGroupApiService : ITravelGroupApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TravelGroupApiService(IHttpClientFactory clientFactory)
        {
            _httpClientFactory = clientFactory;
        }

        public async Task<ResponseDto?> CreateAsync(TravelGroupDto dto)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("TravelGroupApi");

                var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("/api/travelGroup", content);

                var apiContent = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in API call: {ex.Message}");
                return new ResponseDto { IsSuccess = false, Message = "Failed to create TravelGroup" };
            }
        }
        
        public async Task<TravelGroupDto> GetTravelGroupByName(string travelGroupName)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("TravelGroupApi");
                var response = await client.GetAsync($"/api/travelGroup/getByName/{travelGroupName}");

                if (!response.IsSuccessStatusCode) // Ensure valid response
                {
                    Console.WriteLine($"API Error: {response.StatusCode}");
                    return new TravelGroupDto();
                }

                var apiContent = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

                return resp != null && resp.IsSuccess
                    ? JsonConvert.DeserializeObject<TravelGroupDto>(resp.Result.ToString())
                    : new TravelGroupDto();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in API call: {ex.Message}");
                return new TravelGroupDto();
            }
        }
    }
}
