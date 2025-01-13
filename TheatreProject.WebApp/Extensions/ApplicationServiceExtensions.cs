using TheatreProject.WebApp.Constants;
using TheatreProject.WebApp.Services;
using TheatreProject.WebApp.Services.Interfaces;

namespace TheatreProject.WebApp.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IPerformanceService, PerformanceService>();
        services.AddHttpClient<ICartService, CartService>();
        services.AddHttpClient<ICouponService, CouponService>();
        
        Const.PerformanceAPIBase = configuration["ServiceUrls:PerformanceAPI"];
        Const.ShoppingCartAPIBase = configuration["ServiceUrls:ShoppingCartAPI"];
        Const.CouponAPIBase = configuration["ServiceUrls:CouponAPI"];
        
        services.AddScoped<IBaseService, BaseService>();
        services.AddScoped<IPerformanceService, PerformanceService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<ICouponService, CouponService>();
        services.AddControllersWithViews();

        return services;
    }
}