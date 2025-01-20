using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using TheatreProject.GatewaySolution.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddIdentityServices();
builder.Services.AddOcelot();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapGet("/", () => "Theatre Project Gateway API");
app.UseCors("CorsPolicy");

await app.UseOcelot();

app.Run();
