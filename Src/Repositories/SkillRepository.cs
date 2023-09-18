using System.Data;
using api.Data;
using api.Models;
using api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories;

public class SkillRepository : ISkillRepository
{
    private readonly DataContext _dataContext;

    public SkillRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    public async Task<IEnumerable<Skill>> GetUserSkillsAsync(int userId, int skillTypeId)
    {
        return await _dataContext
            .Skills
            .Where(x => x.UserId == userId && x.SkillTypeId == skillTypeId)
            .ToListAsync();
    }
}