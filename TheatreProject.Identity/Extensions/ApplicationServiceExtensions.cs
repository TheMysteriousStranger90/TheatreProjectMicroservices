using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheatreProject.Identity.Data;
using TheatreProject.Identity.Filters;
using TheatreProject.Identity.Helpers;
using TheatreProject.Identity.Models;
using TheatreProject.Identity.Services;
using TheatreProject.Identity.Services.Interfaces;

namespace TheatreProject.Identity.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(opt =>
        {
            opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddSingleton<IRemoteHostService, RemoteHostService>();
        services.AddSingleton<IIpBlockingService, IpBlockingService>();

        services.AddScoped<IpBlockActionFilter>();

        services.AddTransient<IPasswordHasher<ApplicationUser>>(provider =>
        {
            var innerHasher = new PasswordHasher<ApplicationUser>();
            var logger = provider.GetRequiredService<ILogger<LoggingPasswordHasher<ApplicationUser>>>();
            return new LoggingPasswordHasher<ApplicationUser>(innerHasher, logger);
        });

        services.AddLogging();

        return services;
    }
}