using TheatreProject.WebApp.Constants;
using TheatreProject.WebApp.Services;
using TheatreProject.WebApp.Services.Interfaces;

namespace TheatreProject.WebApp.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        /*
        services.AddHttpContextAccessor();
        services.AddHttpClient();
        
        services.AddHttpClient("TheatreProjectAPI", client =>
        {
            client.BaseAddress = new Uri(configuration["ServiceUrls:PerformanceAPI"]);
        });
        */
        services.AddHttpClient<IPerformanceService, PerformanceService>();
        
        Const.PerformanceAPIBase = configuration["ServiceUrls:PerformanceAPI"];
        
        //services.AddScoped<IBaseService, BaseService>();
        services.AddScoped<IPerformanceService, PerformanceService>();
        services.AddControllersWithViews();

        return services;
    }
}