namespace api.Models;

public class Skill
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int SkillTypeId { get; set; }
    public SkillType SkillType { get; set; } = null!;
}