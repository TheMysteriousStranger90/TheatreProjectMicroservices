using Microsoft.EntityFrameworkCore;
using TheatreProject.ShoppingCartAPI.Data;
using TheatreProject.ShoppingCartAPI.Mapping;

namespace TheatreProject.ShoppingCartAPI.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        
        services.AddAutoMapper(typeof(AutoMapperProfile));

        return services;
    }
}