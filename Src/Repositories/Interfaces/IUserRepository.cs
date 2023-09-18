using api.Models;

namespace api.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserById(int id);
    Task<User?> GetUserByUsername(string username);
    Task<User?> GetUserByEmail(string email);
    void AddUser(User user);
    Task<bool> SaveAllAsync();
}