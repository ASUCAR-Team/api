using api.DTOs.User;
using api.Extensions;
using api.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Authorize]
public class UserController : BaseApiController
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPut("edit-profile")]
    public async Task<ActionResult> UpdateProfile([FromBody] UpdateProfileDto updateProfileDto)
    {
        var dateNow = DateOnly.FromDateTime(DateTime.UtcNow);
        
        if (0 < updateProfileDto.Birthdate.CompareTo(dateNow))
            return BadRequest();
        
        var user = await _userRepository.GetUserById(User.GetUserId());

        if (user is null)
            return BadRequest();

        var otherUser = await _userRepository.GetUserByEmail(updateProfileDto.Email);

        if (otherUser is not null && otherUser.Id != user.Id)
            return BadRequest();

        user.Name = updateProfileDto.Name.ToLower();
        user.LastName = updateProfileDto.LastName.ToLower();
        user.Birthdate = updateProfileDto.Birthdate;
        user.Email = updateProfileDto.Email.ToLower();
        
        return Ok();
    }
}