namespace TripTracker.Web.Utility
{
    public class StaticDetail
    {
        public static string? TravelGroupApiBasePath { get; set; } = "";
        public static string? AuthApiBasePath { get; set; } = "";
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
