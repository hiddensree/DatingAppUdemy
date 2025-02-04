using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService(IConfiguration config) : ITokenService
{
    public string CreateToken(AppUser user)
    {
        var tokenKey = config["TokenKey"] ?? throw new ArgumentNullException("TokenKey is missing from appsettings.json");

        if (tokenKey.Length < 64)throw new ArgumentException("TokenKey must be at least 16 characters long");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)); // Create a key from the token key. Use the key to sign the credentials.

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Username) // Create a claim with the user's username.
        };

        var tokenDescriptor = new SecurityTokenDescriptor // Create a token descriptor with the claims, expiration date, and signing credentials.
        {
            Subject = new ClaimsIdentity(claims), // Create a claim with the user's username.
            Expires = DateTime.UtcNow.AddDays(7), // Set the expiration date of the token.
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature) // Sign the credentials with the key. Token key length must be at least 64 characters long.
        };

        var tokenHandler = new JwtSecurityTokenHandler(); // Create a token handler.
        var token = tokenHandler.CreateToken(tokenDescriptor); // Create a token with the token descriptor.

        return tokenHandler.WriteToken(token); // Return the token.
        
    }
}
