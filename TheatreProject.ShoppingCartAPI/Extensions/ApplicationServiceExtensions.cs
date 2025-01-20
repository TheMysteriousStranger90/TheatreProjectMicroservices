using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TheatreProject.MessageBus;
using TheatreProject.ShoppingCartAPI.Data;
using TheatreProject.ShoppingCartAPI.Mapping;
using TheatreProject.ShoppingCartAPI.Repositories;
using TheatreProject.ShoppingCartAPI.Repositories.Interfaces;
using TheatreProject.ShoppingCartAPI.Services;
using TheatreProject.ShoppingCartAPI.Services.Interfaces;
using TheatreProject.ShoppingCartAPI.Utility;
using TheatreProject.ShoppingCartAPI.Validators;

namespace TheatreProject.ShoppingCartAPI.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddAutoMapper(typeof(AutoMapperProfile));

        services.AddScoped<ValidationFilter>();
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<CartDtoValidator>();

        services.AddScoped<ICartRepository, CartRepository>();

        services.AddScoped<BackendApiAuthenticationHttpClientHandler>();
        services.AddScoped<ICouponService, CouponService>();

        services.AddSingleton<IMessageBus, MessageBus.MessageBus>();

        services.AddHttpClient("PerformanceAPI", u => u.BaseAddress =
                new Uri(configuration["ServiceUrls:PerformanceAPI"]))
            .AddHttpMessageHandler<BackendApiAuthenticationHttpClientHandler>();


        services.AddHttpClient("CouponAPI", u =>
        {
            u.BaseAddress = new Uri(configuration["ServiceUrls:CouponAPI"]);
            u.Timeout = TimeSpan.FromSeconds(30);
        }).AddHttpMessageHandler<BackendApiAuthenticationHttpClientHandler>();


        return services;
    }
}