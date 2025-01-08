using TheatreProject.WebApp.Constants;
using TheatreProject.WebApp.Services;
using TheatreProject.WebApp.Services.Interfaces;

namespace TheatreProject.WebApp.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IPerformanceService, PerformanceService>();
        
        Const.PerformanceAPIBase = configuration["ServiceUrls:PerformanceAPI"];
        
        services.AddScoped<IPerformanceService, PerformanceService>();
        services.AddControllersWithViews();

        return services;
    }
}