using Newtonsoft.Json;
using System.Net;
using System.Text;
using TripTracker.Web.Models.Dto;
using TripTracker.Web.Service.Interface;
using static TripTracker.Web.Utility.StaticDetail;

namespace TripTracker.Web.Service
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenHandler _tokenHandler;

        public BaseService(IHttpClientFactory httpClientFactory, ITokenHandler tokenHandler)
        {
            _httpClientFactory = httpClientFactory;
            _tokenHandler = tokenHandler;
        }
        public async Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = true)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("TripTrackerAPI");

                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");

                if (withBearer)
                {
                    var token = _tokenHandler.GetToken();
                    if (!string.IsNullOrEmpty(token))
                    {
                        message.Headers.Add("Authorization", $"Bearer {token}");
                    }
                }

                message.RequestUri = new Uri(requestDto.Url);

                message.Method = requestDto.ApiType switch
                {
                    ApiType.POST => HttpMethod.Post,
                    ApiType.PUT => HttpMethod.Put,
                    ApiType.DELETE => HttpMethod.Delete,
                    _ => HttpMethod.Get
                };

                if (requestDto.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
                }

                HttpResponseMessage responseMessage = new HttpResponseMessage();

                Console.WriteLine($"----------- Request url :: {message.RequestUri}");

                responseMessage = await client.SendAsync(message);

                string apiContent = "";
                ResponseDto? apiResponseDto = new ResponseDto();

                switch (responseMessage.StatusCode)
                {

                    case HttpStatusCode.NotFound:
                        return new ResponseDto() { IsSuccess = false, Message = "Not found" };
                    case HttpStatusCode.BadRequest:
                        apiContent = await responseMessage.Content.ReadAsStringAsync();
                        apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                        return new ResponseDto() { IsSuccess = false, Message = apiResponseDto!.Message };
                    case HttpStatusCode.InternalServerError:
                        return new ResponseDto() { IsSuccess = false, Message = "Internal Server Error" };
                    case HttpStatusCode.Unauthorized:
                        return new ResponseDto() { IsSuccess = false, Message = "Unauthorized" };
                    case HttpStatusCode.Forbidden:
                        return new ResponseDto() { IsSuccess = false, Message = "Forbidden" };
                    case HttpStatusCode.BadGateway:
                        return new ResponseDto() { IsSuccess = false, Message = "Bad Gateway" };
                    default:
                        apiContent = await responseMessage.Content.ReadAsStringAsync();
                        apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                        return apiResponseDto;
                }
            }
            catch (Exception ex)
            {
                return new ResponseDto() { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}
