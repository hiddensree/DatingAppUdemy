using API.Data;
using API.Extensions;
using API.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration); // cleanest way to add services to the container
builder.Services.AddIdentityServices(builder.Configuration); 

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200", "https://localhost:4200"));

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope(); // Create a scope for the service provider
var services = scope.ServiceProvider; // Get the service provider
try
{
    var context = services.GetRequiredService<DataContext>(); // Get the DataContext service
    await context.Database.MigrateAsync(); // Apply any pending migrations
    await Seed.SeedUsers(context); // Seed the users data
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>(); // Get the logger service
    logger.LogError(ex, "An error occurred during migration");
}

app.Run();