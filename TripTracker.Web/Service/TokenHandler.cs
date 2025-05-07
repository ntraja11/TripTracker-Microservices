using TripTracker.Web.Service.Interface;
using TripTracker.Web.Utility;

namespace TripTracker.Web.Service
{
    public class TokenHandler : ITokenHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TokenHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string? GetToken()
        {
            string? token = null;

            bool hasToken = _httpContextAccessor.HttpContext?.Request.Cookies.TryGetValue(StaticDetail.TokenCookieName, out token) == true;

            return hasToken ? token : null;
        }

        public void RemoveToken()
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete(StaticDetail.TokenCookieName);
        }

        public void SetToken(string token)
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Append(StaticDetail.TokenCookieName, token);
        }
    }
}
