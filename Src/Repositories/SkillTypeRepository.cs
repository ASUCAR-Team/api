using api.Data;
using api.Models;
using api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories;

public class SkillTypeRepository : ISkillTypeRepository
{
    private readonly DataContext _dataContext;

    public SkillTypeRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<SkillType?> GetSkillTypeAsync(string name)
    {
        return await _dataContext.SkillTypes.SingleOrDefaultAsync(s => s.Name == name);
    }
}