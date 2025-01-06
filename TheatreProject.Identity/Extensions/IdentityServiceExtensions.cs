using Duende.IdentityServer.AspNetIdentity;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using TheatreProject.Identity.Constants;
using TheatreProject.Identity.Data;
using TheatreProject.Identity.Initializer;
using TheatreProject.Identity.Models;
using TheatreProject.Identity.Services;

namespace TheatreProject.Identity.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

        var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.EmitStaticAudienceClaim = true;
            })
            .AddInMemoryIdentityResources(Const.IdentityResources)
            .AddInMemoryApiScopes(Const.ApiScopes)
            .AddInMemoryClients(Const.Clients)
            .AddAspNetIdentity<ApplicationUser>();

        services.AddScoped<IDbInitializer, DbInitializer>();
        services.AddScoped<IProfileService, ProfileService>();

        builder.AddDeveloperSigningCredential();

        return services;
    }
}