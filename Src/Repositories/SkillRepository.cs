using System.Data;
using api.Data;
using api.DTOs.Skill;
using api.Models;
using api.Repositories.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories;

public class SkillRepository : ISkillRepository
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public SkillRepository(
        DataContext dataContext,
        IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<Skill>> GetUserSkillsAsync(int userId, int skillTypeId)
    {
        return await _dataContext
            .Skills
            .Where(x => x.UserId == userId && x.SkillTypeId == skillTypeId)
            .ToListAsync();
    }

    public async Task<IEnumerable<SkillDto>> GetUserSkillsDtoAsync(int userId, int skillTypeId)
    {
        return await _dataContext
            .Skills
            .Where(x => x.UserId == userId && x.SkillTypeId == skillTypeId)
            .ProjectTo<SkillDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }
}