using API.DTOs;
using API.Entities;

namespace API.Interfaces;

public interface IUserRepository
{
    void Update(AppUser user);
    Task<bool> SaveAllAsync(); // Save changes to the database
    Task<IEnumerable<AppUser>> GetUsersAsync(); // Get all users
    Task<AppUser?> GetUserByIdAsync(int id); // Get a user by id
    Task<AppUser?> GetUserByUsernameAsync(string username); // Get a user by username

    Task<IEnumerable<MemberDto>?> GetMembersAsync(); 
    Task<MemberDto?> GetMemberAsync(string username);
}