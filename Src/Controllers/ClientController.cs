using api.DTOs.Skill;
using api.Extensions;
using api.Models;
using api.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace api.Controllers;

[Authorize]
public class ClientController : BaseApiController
{
    private readonly IRoleRepository _roleRepository;
    private readonly ISkillRepository _skillRepository;
    private readonly ISkillTypeRepository _skillTypeRepository;
    private readonly IUserRepository _userRepository;

    public ClientController(
        IRoleRepository roleRepository,
        ISkillRepository skillRepository,
        ISkillTypeRepository skillTypeRepository,
        IUserRepository userRepository)
    {
        _roleRepository = roleRepository;
        _skillRepository = skillRepository;
        _skillTypeRepository = skillTypeRepository;
        _userRepository = userRepository;
    }

    [HttpPost("add-socio-emotional-skill")]
    public async Task<ActionResult> AddSocioEmotionalSkill(
        [FromBody] List<SkillDto> skillsDto)
    {
        var user = await _userRepository.GetUserByIdAsync(User.GetUserId());
        var role = await _roleRepository.GetRoleByNameAsync("client");

        if (user is null || user.DisabledAt != DateTime.MinValue || user.RoleId != role.Id)
            return BadRequest();
            
        if (skillsDto.IsNullOrEmpty())
            return BadRequest();

        var skillType = await _skillTypeRepository.GetSkillTypeByNameAsync("socio-emotional");

        if (skillType is null)
            return BadRequest();

        var skills = await _skillRepository.GetUserSkillsAsync(User.GetUserId(), skillType.Id);

        if (skills.Any(skill => skillsDto.Exists(x => x.Name == skill.Name)))
            return BadRequest();

        foreach (var skillDto in skillsDto)
        {
            user.Skills.Add(new Skill
            {
                Name = skillDto.Name.ToLower(),
                User = user,
                SkillType = skillType
            });
        }

        if (!await _userRepository.SaveAllAsync())
            return BadRequest();
        
        return Ok();
    }
    
    [HttpPost("add-technical-skill")]
    public async Task<ActionResult> AddTechnicalSkill(
        [FromBody] List<SkillDto> skillsDto)
    {
        var user = await _userRepository.GetUserByIdAsync(User.GetUserId());
        var role = await _roleRepository.GetRoleByNameAsync("client");

        if (user is null || user.DisabledAt != DateTime.MinValue || user.RoleId != role.Id)
            return BadRequest();
            
        if (skillsDto.IsNullOrEmpty())
            return BadRequest();

        var skillType = await _skillTypeRepository.GetSkillTypeByNameAsync("technical");

        if (skillType is null)
            return BadRequest();

        var skills = await _skillRepository.GetUserSkillsAsync(User.GetUserId(), skillType.Id);

        if (skills.Any(skill => skillsDto.Exists(x => x.Name == skill.Name)))
            return BadRequest();

        foreach (var skillDto in skillsDto)
        {
            user.Skills.Add(new Skill
            {
                Name = skillDto.Name.ToLower(),
                User = user,
                SkillType = skillType
            });
        }

        if (!await _userRepository.SaveAllAsync())
            return BadRequest();
        
        return Ok();
    }
}