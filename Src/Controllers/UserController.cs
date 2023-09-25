using api.DTOs.Post;
using api.DTOs.User;
using api.Extensions;
using api.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Authorize]
public class UserController : BaseApiController
{
    private readonly ISkillRepository _skillRepository;
    private readonly ISkillTypeRepository _skillTypeRepository;
    private readonly IUserRepository _userRepository;

    public UserController(
        ISkillRepository skillRepository,
        ISkillTypeRepository skillTypeRepository,
        IUserRepository userRepository)
    {
        _skillRepository = skillRepository;
        _skillTypeRepository = skillTypeRepository;
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserWithSkillsDto>>> GetUsers()
    {
        var users = await _userRepository.GetUsersAsync();
        return Ok(users);
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<UserWithSkillsDto>> GetUserById(
        [FromRoute] int userId)
    {
        var user = await _userRepository.GetUserDtoByIdAsync(userId);

        if (user is null)
            return BadRequest();
        
        var skillType = await _skillTypeRepository.GetSkillTypeByNameAsync("socio-emotional");

        if (skillType is null)
            return BadRequest();

        user.SocioEmotionalSkills = await _skillRepository.GetUserSkillsDtoAsync(userId, skillType.Id);
        
        skillType = await _skillTypeRepository.GetSkillTypeByNameAsync("technical");

        if (skillType is null)
            return BadRequest();

        user.TechnicalSkills = await _skillRepository.GetUserSkillsDtoAsync(userId, skillType.Id);

        return Ok(user);
    }

    [HttpPut("edit-profile")]
    public async Task<ActionResult> UpdateProfile(
        [FromBody] UpdateProfileDto updateProfileDto)
    {
        var dateNow = DateOnly.FromDateTime(DateTime.UtcNow);
        
        if (0 < updateProfileDto.Birthdate.CompareTo(dateNow))
            return BadRequest();
        
        var user = await _userRepository.GetUserByIdAsync(User.GetUserId());

        if (user is null || user.DisabledAt != DateTime.MinValue)
            return BadRequest();

        var otherUser = await _userRepository.GetUserByEmailAsync(updateProfileDto.Email);

        if (otherUser is not null && otherUser.Id != user.Id)
            return BadRequest();

        user.Name = updateProfileDto.Name.ToLower();
        user.LastName = updateProfileDto.LastName.ToLower();
        user.Birthdate = updateProfileDto.Birthdate;
        user.Email = updateProfileDto.Email.ToLower();

        if (!await _userRepository.SaveAllAsync())
            return BadRequest();
        
        return Ok();
    }
}