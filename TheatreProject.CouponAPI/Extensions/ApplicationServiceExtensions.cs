using Microsoft.EntityFrameworkCore;
using TheatreProject.CouponAPI.Data;
using TheatreProject.CouponAPI.Mapping;
using TheatreProject.CouponAPI.Repositories;
using TheatreProject.CouponAPI.Repositories.Interfaces;

namespace TheatreProject.CouponAPI.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        
        services.AddAutoMapper(typeof(AutoMapperProfile));
        
        services.AddScoped<ICouponRepository, CouponRepository>();
        


        return services;
    }
}