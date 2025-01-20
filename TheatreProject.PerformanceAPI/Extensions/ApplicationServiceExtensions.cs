using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TheatreProject.PerformanceAPI.Data;
using TheatreProject.PerformanceAPI.Mapping;
using TheatreProject.PerformanceAPI.Repositories;
using TheatreProject.PerformanceAPI.Services;
using TheatreProject.PerformanceAPI.Validators;

namespace TheatreProject.PerformanceAPI.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddAutoMapper(typeof(AutoMapperProfile));

        services.AddFluentValidation(fv => { fv.RegisterValidatorsFromAssemblyContaining<PerformanceDtoValidator>(); });

        services.AddMemoryCache();
        services.AddSingleton<ICacheKeyService, CacheKeyService>();

        services.AddScoped<IPerformanceRepository, PerformanceRepository>();

        services.AddScoped<ValidationFilter>();

        return services;
    }
}