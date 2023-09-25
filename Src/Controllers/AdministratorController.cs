using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using api.DTOs.Administrative;
using api.DTOs.User;
using api.Extensions;
using api.Models;
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
        var user = await _userRepository.GetUserByIdAsync(User.GetUserId());
        var role = await _roleRepository.GetRoleByNameAsync("admin");
        
        if (user is null || user.RoleId != role.Id)
            return BadRequest();
        
        if (await _userRepository.GetUserByUsernameAsync(registerAdministrativeDto.Username) is not null)
            return BadRequest();
        
        var dateNow = DateOnly.FromDateTime(DateTime.UtcNow);
        
        if (0 < registerAdministrativeDto.Birthdate.CompareTo(dateNow))
            return BadRequest();
        
        role = await _roleRepository.GetRoleByNameAsync("administrative");
        using var hmac = new HMACSHA512();
        var password = GetPassword(registerAdministrativeDto.Email);

        if (password is null)
            return BadRequest();

        user = new User
        {
            Username = registerAdministrativeDto.Username,
            Email = registerAdministrativeDto.Email.ToLower(),
            Name = registerAdministrativeDto.Name.ToLower(),
            LastName = registerAdministrativeDto.LastName.ToLower(),
            Birthdate = registerAdministrativeDto.Birthdate,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
            PasswordSalt = hmac.Key,
            Role = role
        };
        
        _userRepository.AddUser(user);

        if (!await _userRepository.SaveAllAsync())
            return BadRequest();
        
        return Ok();
    }

    [HttpGet("users")]
    public async Task<ActionResult<IEnumerable<UserWithStatusDto>>> GetUsers()
    {
        var user = await _userRepository.GetUserByIdAsync(User.GetUserId());
        var role = await _roleRepository.GetRoleByNameAsync("admin");
        
        if (user is null || user.RoleId != role.Id)
            return BadRequest();

        var users = await _userRepository.GetUserWithStatusDtosAsync();

        if (users is not null)
            users = users.Where(x => x.Role.Name != "admin");

        return Ok(users);
    }

    [HttpPut("users/{userId}")]
    public async Task<ActionResult> ChangeUserStatus(
        [FromRoute] int userId)
    {
        var user = await _userRepository.GetUserByIdAsync(User.GetUserId());
        var role = await _roleRepository.GetRoleByNameAsync("admin");
        
        if (user is null || user.RoleId != role.Id)
            return BadRequest();
        
        user = await _userRepository.GetUserByIdAsync(userId);

        if (user is null || user.RoleId == role.Id)
            return BadRequest();
        
        if (user.DisabledAt == DateTime.MinValue)
            user.DisabledAt = DateTime.UtcNow;
        else
            user.DisabledAt = DateTime.MinValue;

        if (!await _userRepository.SaveAllAsync())
            return BadRequest();
        
        return Ok();
    }

    private string? GetPassword([EmailAddress] string email)
    {
        int index = email.IndexOf("@");

        if (0 < index)
            return email.Substring(0, index);
        
        else if (index == 0)
            return null;

        throw new ArgumentException();
    }
}