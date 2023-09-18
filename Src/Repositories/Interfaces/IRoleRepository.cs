using api.Models;

namespace api.Repositories.Interfaces;

public interface IRoleRepository
{
    Task<Role> GetRoleByNameAsync(string name);
}