using api.Data;
using api.DTOs.Comment;
using api.Repositories.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace api.Src.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;
    
    public CommentRepository(
        DataContext dataContext,
        IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CommentDto>> GetCommentsByPostIdAsync(int postId)
    {
        return await _dataContext
            .Comments
            .Where(c => c.PostId == postId)
            .ProjectTo<CommentDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }
}