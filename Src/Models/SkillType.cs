namespace api.Models;

public class SkillType
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    
    public List<Skill> Skills { get; set; } = new();
}