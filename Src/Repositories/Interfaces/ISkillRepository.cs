using api.Models;

namespace api.Repositories.Interfaces;

public interface ISkillRepository
{
    Task<IEnumerable<Skill>> GetUserSkillsAsync(int userId, int skillTypeId);
}