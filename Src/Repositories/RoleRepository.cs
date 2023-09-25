using api.Data;
using api.Models;
using api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly DataContext _dataContext;

    public RoleRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<Role> GetRoleByNameAsync(string name)
    {
        return await _dataContext.Roles.SingleAsync(x => x.Name == name);
    }

    public async Task<Role> GetRoleByRoleIdAsync(int id)
    {
        return await _dataContext.Roles.SingleAsync(x => x.Id == id);
    }
}