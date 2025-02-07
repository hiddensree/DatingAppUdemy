using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(DataContext context, ITokenService tokenService) : BaseApiController
{
    [HttpPost("register")] // /account/register
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await UserExists(registerDto.Username))
            return BadRequest("Username is taken");

        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            Username = registerDto.Username,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key,
            KnownAs = registerDto.KnownAs,
            Gender = registerDto.Gender,
            City = registerDto.City,
            Country = registerDto.Country
        };

        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return new UserDto
        {
            Username = user.Username,
            Token = tokenService.CreateToken(user)
        };
    }

    [HttpPost("login")] // /account/login
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await context.Users
            .Include(p => p.Photos)
            .FirstOrDefaultAsync(x => x.Username.ToLower() == loginDto.Username.ToLower()); // Get the user.
        if (user == null)
            return Unauthorized("Invalid username");

        using var hmac = new HMACSHA512(user.PasswordSalt); // key is the salt
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for (int i = 0; i < computedHash.Length; i++) // Compare the computed hash with the stored hash. If they are not equal, return Unauthorized.
        {
            if (computedHash[i] != user.PasswordHash[i])
                return Unauthorized("Invalid password");
        }

         return new UserDto
        {
            Username = user.Username,
            Token = tokenService.CreateToken(user),
            PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
        };
    }

    /// <summary>
    /// Check if the user exists.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    private async Task<bool> UserExists(string username)
    {
        return await context.Users.AnyAsync(x => x.Username.ToLower() == username.ToLower()); // Bob == bob
    }

}