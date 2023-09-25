using api.Data;
using api.Models;

namespace api.Src.Repositories.Interfaces;

public interface ILikeRepository
{
    Task<Like?> GetLikeByPostIdAndUserIdAsync(int postId, int userId);
}