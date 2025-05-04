using Microsoft.AspNetCore.Mvc;
using TripTracker.Web.Utility;
using static TripTracker.Web.Utility.StaticDetail;

namespace TripTracker.Web.Models.Dto
{
    public class RequestDto
    {
        public Object? Data { get; set; }
        public ApiType ApiType { get; set; } = ApiType.GET;

        public string Url { get; set; };
    }
}
