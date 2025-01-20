using Microsoft.EntityFrameworkCore;
using TheatreProject.EmailAPI.Data;
using TheatreProject.EmailAPI.Helpers;
using TheatreProject.EmailAPI.Mapping;
using TheatreProject.EmailAPI.Messaging;
using TheatreProject.EmailAPI.Services;
using TheatreProject.EmailAPI.Services.Interfaces;

namespace TheatreProject.EmailAPI.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.Configure<MailSettings>(configuration.GetSection("MailSettings"));

        services.AddScoped<IEmailService, EmailService>();
        services.AddSingleton<IEmailServiceFactory, EmailServiceFactory>();
        services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();

        services.AddAutoMapper(typeof(AutoMapperProfile));

        services.AddLogging(config =>
        {
            config.AddConsole();
            config.AddDebug();
        });

        return services;
    }
}