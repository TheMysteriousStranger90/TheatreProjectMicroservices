using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Stripe.Climate;
using TheatreProject.OrderAPI.Data;
using TheatreProject.OrderAPI.Mapping;
using TheatreProject.OrderAPI.Repositories;
using TheatreProject.OrderAPI.Repositories.Interfaces;
using TheatreProject.OrderAPI.Services;
using TheatreProject.OrderAPI.Services.Interfaces;
using TheatreProject.OrderAPI.Utility;

namespace TheatreProject.OrderAPI.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        
        services.AddAutoMapper(typeof(AutoMapperProfile));
        
        services.AddHttpClient<IPerformanceService, PerformanceService>();
        
        services.AddScoped<IOrderRepository, OrderRepository>();
        
        services.AddScoped<BackendApiAuthenticationHttpClientHandler>();
        services.AddScoped<IPerformanceService, PerformanceService>();
        
        services.AddHttpClient("PerformanceAPI", u => u.BaseAddress =
            new Uri(configuration["ServiceUrls:PerformanceAPI"])).AddHttpMessageHandler<BackendApiAuthenticationHttpClientHandler>();

        return services;
    }
}