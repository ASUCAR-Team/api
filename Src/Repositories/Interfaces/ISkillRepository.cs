using api.DTOs.Skill;
using api.Models;

namespace api.Repositories.Interfaces;

public interface ISkillRepository
{
    Task<IEnumerable<Skill>> GetUserSkillsAsync(int userId, int skillTypeId);
    Task<IEnumerable<SkillDto>> GetUserSkillsDtoAsync(int userId, int skillTypeId);
}