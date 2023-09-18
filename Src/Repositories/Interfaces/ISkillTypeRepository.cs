using api.Models;

namespace api.Repositories.Interfaces;

public interface ISkillTypeRepository
{
    Task<SkillType?> GetSkillTypeAsync(string name);
}