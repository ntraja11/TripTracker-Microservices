using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System.Text;
using TripTracker.Services.EmailApi.Models.Dto;
using TripTracker.Services.EmailApi.Services;

namespace TripTracker.Services.EmailApi.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly IConfiguration _configuration;
        private readonly string serviceBusConnectionString;
        private readonly string emailQueueName;
        private ServiceBusProcessor _emailQueueProcessor;
        private readonly EmailService _emailService;
        public AzureServiceBusConsumer(IConfiguration configuration, EmailService emailService)
        {
            _configuration = configuration;
            serviceBusConnectionString = _configuration.GetValue<string>("ConnectionStrings:ServiceBusConnection");
            emailQueueName = _configuration.GetValue<string>("QueueNames:EmailTripCreated");

            var client = new ServiceBusClient(serviceBusConnectionString);

            _emailQueueProcessor = client.CreateProcessor(emailQueueName);
            _emailService = emailService;
        }

        public async Task StartAsync()
        {
            _emailQueueProcessor.ProcessMessageAsync += OnEmailCreatedMessageHandler;
            _emailQueueProcessor.ProcessErrorAsync += OnErrorHandler;
            await _emailQueueProcessor.StartProcessingAsync();
        }

        private async Task OnErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine($"Error processing message: {args.Exception.Message}");
            await Task.CompletedTask;
        }

        private async Task OnEmailCreatedMessageHandler(ProcessMessageEventArgs args)
        {
            var emailMessage = Encoding.UTF8.GetString(args.Message.Body);

            TripDto tripDto = JsonConvert.DeserializeObject<TripDto>(emailMessage);

            try
            {
                _emailService.SendTripCreatedEmail(tripDto!);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
            }
        }

        public async Task StopAsync()
        {
            await _emailQueueProcessor.StopProcessingAsync();
            await _emailQueueProcessor.DisposeAsync();
        }
    }
}
