using System.Text;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using TheatreProject.EmailAPI.Models.DTOs;
using TheatreProject.EmailAPI.Services.Interfaces;

namespace TheatreProject.EmailAPI.Messaging;

public class AzureServiceBusConsumer : IAzureServiceBusConsumer
{
    private readonly IConfiguration _configuration;
    private readonly IEmailServiceFactory _emailServiceFactory;
    private ServiceBusProcessor _orderPaymentProcessor;
    private readonly string serviceBusConnectionString;
    private readonly string orderPaymentTopic;

    private bool _isServiceBusEnabled;
    private readonly ILogger<AzureServiceBusConsumer> _logger;

    public AzureServiceBusConsumer(IConfiguration configuration, IEmailServiceFactory emailServiceFactory,
        ILogger<AzureServiceBusConsumer> logger)
    {
        _configuration = configuration;
        _emailServiceFactory = emailServiceFactory;
        serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
        orderPaymentTopic = _configuration.GetValue<string>("TopicAndQueueNames:OrderPaymentTopic");
        _logger = logger;

        InitializeServiceBus(serviceBusConnectionString);
    }

    private void InitializeServiceBus(string connectionString)
    {
        try
        {
            if (string.IsNullOrEmpty(connectionString) || connectionString.Contains("your-service-bus"))
            {
                _isServiceBusEnabled = false;
                _logger.LogWarning("Service Bus is disabled: Connection string not properly configured");
                return;
            }

            var client = new ServiceBusClient(connectionString);
            _orderPaymentProcessor = client.CreateProcessor(orderPaymentTopic);
            _isServiceBusEnabled = true;
        }
        catch (Exception ex)
        {
            _isServiceBusEnabled = false;
            _logger.LogWarning(ex, "Failed to initialize Service Bus. Running in limited mode");
        }
    }

    public Task Start()
    {
        if (!_isServiceBusEnabled)
        {
            _logger.LogInformation("Service Bus consumer not started: Service Bus is disabled");
            return Task.CompletedTask;
        }

        try
        {
            _orderPaymentProcessor.ProcessMessageAsync += ProcessOrderPayment;
            _orderPaymentProcessor.ProcessErrorAsync += ErrorHandler;
            return _orderPaymentProcessor.StartProcessingAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting Service Bus processor");
            return Task.CompletedTask;
        }
    }

    public async Task Stop()
    {
        if (_isServiceBusEnabled && _orderPaymentProcessor != null)
        {
            await _orderPaymentProcessor.StopProcessingAsync();
            await _orderPaymentProcessor.DisposeAsync();
        }
    }

    private async Task ProcessOrderPayment(ProcessMessageEventArgs args)
    {
        var message = args.Message;
        var body = Encoding.UTF8.GetString(message.Body);
        var orderConfirmation = JsonConvert.DeserializeObject<OrderConfirmationDto>(body);

        try
        {
            var emailService = _emailServiceFactory.CreateEmailService();
            await emailService.SendOrderConfirmationAsync(orderConfirmation);
            await args.CompleteMessageAsync(args.Message);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine(args.Exception.ToString());
        return Task.CompletedTask;
    }
}