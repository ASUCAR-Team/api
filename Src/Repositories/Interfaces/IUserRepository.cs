using api.DTOs.User;
using api.Models;

namespace api.Repositories.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<UserDto>> GetUsersAsync();
    Task<User?> GetUserByIdAsync(int id);
    Task<UserWithSkillsDto?> GetUserDtoByIdAsync(int id);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<User?> GetUserByEmailAsync(string email);
    void AddUser(User user);
    Task<bool> SaveAllAsync();
    Task<IEnumerable<UserWithStatusDto>> GetUserWithStatusDtosAsync();
}