using api.DTOs.Post;
using api.Models;

namespace api.Repositories.Interfaces;

public interface IPostRepository
{
    Task<Post?> GetPostByIdAsync(int id);
    Task<IEnumerable<PostDto>> GetPostsAsync();
}