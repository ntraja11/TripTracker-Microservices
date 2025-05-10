namespace TripTracker.Web.Utility
{
    public class StaticDetail
    {
        public static string? TravelGroupApiBasePath { get; set; } = "";
        public static string? TripApiBasePath { get; set; } = "";
        public static string? ParticipantApiBasePath { get; set; } = "";
        public static string? ExpenseApiBasePath { get; set; } = "";
        public static string? AuthApiBasePath { get; set; } = "";

        public const string RoleAdmin = "ADMIN";
        public const string RoleMember = "MEMBER";
        public const string TokenCookieName = "JwtToken";

        public const string TripStatusPlanned = "Planned";
        public const string TripStatusInProgress = "In Progress";
        public const string TripStatusCompleted = "Completed";
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
