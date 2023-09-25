using api.Data;
using api.DTOs.Post;
using api.Models;
using api.Repositories.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories;

public class PostRepository : IPostRepository
{
    private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
    
    public PostRepository(
        DataContext dataContext,
        IMapper mapper)
    {
        _mapper = mapper;
        _dataContext = dataContext;
    }

    public async Task<Post?> GetPostByIdAsync(int id)
    {
        return await _dataContext
            .Posts
            .Include(p => p.Likes)
            .SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<PostDto>> GetPostsAsync()
    {
        return await _dataContext
            .Posts
            .ProjectTo<PostDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }
}