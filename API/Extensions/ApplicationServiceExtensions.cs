using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{
    /// <summary>
    /// Add services to the container.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
         IConfiguration config)
    {
        // Add services to the container.
        services.AddControllers();
        services.AddDbContext<DataContext>(opt =>
        {
            opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });
        services.AddCors(); // Allow cross-origin requests
        services.AddScoped<ITokenService, TokenService>(); // Each time ITokenService is requested, a new instance of TokenService is created 
        services.AddScoped<IUserRepository, UserRepository>(); // Each time IUserRepository is requested, a new instance of UserRepository is created
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // Add AutoMapper to the project

        return services;
    }
}
