using api.DTOs.Administrative;
using api.Extensions;
using api.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Authorize]
public class AdministratorController : BaseApiController
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;

    public AdministratorController(IUserRepository userRepository, IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    [HttpPost("register-administrative")]
    public async Task<ActionResult> RegisterAdministrative(
        [FromBody] RegisterAdministrativeDto registerAdministrativeDto)
    {
        var dateNow = DateOnly.FromDateTime(DateTime.UtcNow);
        
        if (0 < registerAdministrativeDto.Birthdate.CompareTo(dateNow))
            return BadRequest();
        
        var user = await _userRepository.GetUserById(User.GetUserId());

        if (user is null || user.Role.Name != "admin")
            return BadRequest();
        
        return Ok();
    }
}