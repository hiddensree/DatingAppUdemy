using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(opt =>
                {
                    var tokenKey = config["TokenKey"] ?? throw new ArgumentNullException("TokenKey not found in appsettings.json");
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true, // Validate the server that created that token
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)), // The key that will be used to validate the token
                        ValidateIssuer = false, // Validate the user that created the token
                        ValidateAudience = false // Validate the user that is using the token
                    };
                });

        return services;
    }
}