namespace TripTracker.Services.EmailApi.Messaging
{
    public interface IAzureServiceBusConsumer
    {
        Task StartAsync();
        Task StopAsync();
    }
}
