using System.Security.Cryptography;
using System.Text;
using api.DTOs.User;
using api.Extensions;
using api.Models;
using api.Repositories.Interfaces;
using api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Authorize]
public class AuthController : BaseApiController
{
    private readonly IRoleRepository _roleRepository;
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;

    public AuthController(
        IRoleRepository roleRepository,
        ITokenService tokenService,
        IUserRepository userRepository)
    {
        _roleRepository = roleRepository;
        _tokenService = tokenService;
        _userRepository = userRepository;
    }
    
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<UserTokenDto>> Register(
        [FromBody] RegisterClientDto registerClientDto)
    {
        var dateNow = DateOnly.FromDateTime(DateTime.UtcNow);
        
        if (await _userRepository.GetUserByUsernameAsync(registerClientDto.Username) != null)
            return BadRequest();

        if (await _userRepository.GetUserByEmailAsync(registerClientDto.Email.ToLower()) != null)
            return BadRequest();

        if (0 < registerClientDto.Birthdate.CompareTo(dateNow))
            return BadRequest();

        if (registerClientDto.Password != registerClientDto.ConfirmPassword)
            return BadRequest();

        using var hmac = new HMACSHA512();

        var role = await _roleRepository.GetRoleByNameAsync("client");

        var user = new User
        {
            Username = registerClientDto.Username,
            Email = registerClientDto.Email.ToLower(),
            Name = registerClientDto.Name.ToLower(),
            LastName = registerClientDto.LastName.ToLower(),
            Birthdate = registerClientDto.Birthdate,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerClientDto.Password)),
            PasswordSalt = hmac.Key,
            Role = role
        };
        
        _userRepository.AddUser(user);

        if (!await _userRepository.SaveAllAsync())
            return BadRequest();
        
        return new UserTokenDto
        {
            Username = user.Username,
            Token = _tokenService.CreateToken(user)
        };
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<UserTokenDto>> Login(
        [FromBody] LoginDto loginDto)
    {
        var user = await _userRepository.GetUserByEmailAsync(loginDto.Email.ToLower());

        if (user is null || user.DisabledAt != DateTime.MinValue)
            return Unauthorized();

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        if (computedHash.Where((t, i) => t != user.PasswordHash[i]).Any())
            return Unauthorized();

        return new UserTokenDto
        {
            Username = user.Username,
            Token = _tokenService.CreateToken(user)
        };
    }

    [HttpPut("update-password")]
    public async Task<ActionResult> UpdatePassword(
        [FromBody] UpdatePasswordDto updatePasswordDto)
    {
        var user = await _userRepository.GetUserByIdAsync(User.GetUserId());

        if (user is null || user.DisabledAt != DateTime.MinValue)
            return BadRequest();
        
        using var hmac = new HMACSHA512(user.PasswordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(updatePasswordDto.CurrentPassword));

        if (computedHash.Where((t, i) => t != user.PasswordHash[i]).Any())
            return Unauthorized();
        
        user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(updatePasswordDto.NewPassword));

        if (!await _userRepository.SaveAllAsync())
            return BadRequest();
        
        return Ok();
    }
}