using api.Data;
using api.DTOs.User;
using api.Models;
using api.Repositories.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public UserRepository(
        DataContext dataContext,
        IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDto>> GetUsersAsync()
    {
        return await _dataContext
            .Users
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }
    
    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _dataContext.Users.FindAsync(id);
    }

    public async Task<UserWithSkillsDto?> GetUserDtoByIdAsync(int id)
    {
        return await _dataContext
            .Users
            .Where(x => x.Id == id)
            .ProjectTo<UserWithSkillsDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _dataContext.Users.SingleOrDefaultAsync(x => x.Username == username);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
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

    public async Task<IEnumerable<UserWithStatusDto>> GetUserWithStatusDtosAsync()
    {
        return await _dataContext
            .Users
            .ProjectTo<UserWithStatusDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }
}