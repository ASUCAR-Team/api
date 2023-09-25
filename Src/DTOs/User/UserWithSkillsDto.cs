using api.DTOs.Skill;

namespace api.DTOs.User;

public class UserWithSkillsDto
{
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public int Age { get; set; }
    public IEnumerable<SkillDto> SocioEmotionalSkills { get; set; } = null!;
    public IEnumerable<SkillDto> TechnicalSkills { get; set; } = null!;
}