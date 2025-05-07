namespace TripTracker.Web.Service.Interface
{
    public interface ITokenHandler
    {
        string? GetToken();
        void SetToken(string token);
        void RemoveToken();
    }
}
