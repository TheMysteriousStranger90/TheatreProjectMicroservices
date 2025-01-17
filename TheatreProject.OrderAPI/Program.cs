using System.Text.Json.Serialization;
using Stripe;
using TheatreProject.OrderAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });


// Add services to the container.
//builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add HTTP client services
builder.Services.AddHttpClient();

// Add application services
builder.Services.AddApplicationServices(builder.Configuration);

// Add Authentication
builder.Services.AddJwtBearerServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
StripeConfiguration.ApiKey = builder.Configuration.GetValue<string>("Stripe:SecretKey");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();