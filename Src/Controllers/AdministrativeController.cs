using api.Controllers;
using api.DTOs.User;
using api.Extensions;
using api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Src.Controllers;

public class AdministrativeController : BaseApiController
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    
    public AdministrativeController(
        IUserRepository userRepository,
        IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    [HttpGet("clients")]
    public async Task<ActionResult<IEnumerable<UserWithStatusDto>>> GetUsers()
    {
        var user = await _userRepository.GetUserByIdAsync(User.GetUserId());
        var role = await _roleRepository.GetRoleByNameAsync("administrative");
        
        if (user is null || user.RoleId != role.Id)
            return BadRequest();

        var users = await _userRepository.GetUserWithStatusDtosAsync();

        if (users is not null)
            users = users.Where(x => x.Role.Name == "client");
        
        return Ok(users);
    }

    [HttpPut("clients/{userId}")]
    public async Task<ActionResult> ChangeUserStatus(
        [FromRoute] int userId)
    {
        var user = await _userRepository.GetUserByIdAsync(User.GetUserId());
        var administrativeRole = await _roleRepository.GetRoleByNameAsync("administrative");
        var adminRole = await _roleRepository.GetRoleByNameAsync("admin");
        
        if (user is null || user.RoleId != administrativeRole.Id)
            return BadRequest();
        
        user = await _userRepository.GetUserByIdAsync(userId);

        if (user is null || user.RoleId == administrativeRole.Id || user.RoleId == adminRole.Id)
            return BadRequest();
        
        if (user.DisabledAt == DateTime.MinValue)
            user.DisabledAt = DateTime.UtcNow;
        else
            user.DisabledAt = DateTime.MinValue;

        if (!await _userRepository.SaveAllAsync())
            return BadRequest();
        
        return Ok();
    }
}