using TripTracker.Services.EmailApi.Messaging;

namespace TripTracker.Services.EmailApi.Extensions
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder UseAzureServiceBusConsumer(this IApplicationBuilder app)
        {
            var serviceBusConsumer = app.ApplicationServices.GetRequiredService<IAzureServiceBusConsumer>();
            var hostApplicationLifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

            hostApplicationLifetime.ApplicationStarted.Register(() =>
            {
                _ = Task.Run(() => serviceBusConsumer.StartAsync());
            });

            hostApplicationLifetime.ApplicationStopping.Register(() =>
            {
                _ = Task.Run(() => serviceBusConsumer.StopAsync());
            });

            return app;
        }
    }
}