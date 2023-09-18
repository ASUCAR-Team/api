using api.Data;
using api.Models;
using api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DataContext _dataContext;

    public UserRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    public async Task<User?> GetUserById(int id)
    {
        return await _dataContext.Users.FindAsync(id);
    }

    public async Task<User?> GetUserByUsername(string username)
    {
        return await _dataContext.Users.SingleOrDefaultAsync(x => x.Username == username);
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _dataContext.Users.SingleOrDefaultAsync(x => x.Email == email);
    }

    public void AddUser(User user)
    {
        _dataContext.Users.Add(user);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _dataContext.SaveChangesAsync() > 0;
    }
}