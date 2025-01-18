using TheatreProject.EmailAPI.Messaging;

namespace TheatreProject.EmailAPI.Extensions;

public static class AzureBuilderExtensions
{
    private static IAzureServiceBusConsumer ServiceBusConsumer { get; set; }

    public static IApplicationBuilder UseAzureServiceBusConsumer(this IApplicationBuilder app)
    {
        try
        {
            ServiceBusConsumer = app.ApplicationServices.GetService<IAzureServiceBusConsumer>();
            var hostApplicationLife = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            if (ServiceBusConsumer != null)
            {
                hostApplicationLife?.ApplicationStarted.Register(OnStart);
                hostApplicationLife?.ApplicationStopping.Register(OnStop);
            }
        }
        catch (Exception ex)
        {
            var logger = app.ApplicationServices.GetService<ILogger<AzureServiceBusConsumer>>();
            logger?.LogWarning(ex, "Failed to initialize Azure Service Bus Consumer");
        }

        return app;
    }

    private static void OnStop()
    {
        ServiceBusConsumer.Stop();
    }

    private static void OnStart()
    {
        ServiceBusConsumer.Start();
    }
}