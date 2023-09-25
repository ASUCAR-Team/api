using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using api.Src.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories;

public class LikeRepository : ILikeRepository
{
    private readonly DataContext _dataContext;
    
    public LikeRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<Like?> GetLikeByPostIdAndUserIdAsync(int postId, int userId)
    {
        return await _dataContext
            .Likes
            .SingleOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);
    }
}