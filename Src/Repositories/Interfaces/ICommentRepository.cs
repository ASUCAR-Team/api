using api.DTOs.Comment;

namespace api.Repositories.Interfaces;

public interface ICommentRepository
{
    Task<IEnumerable<CommentDto>> GetCommentsByPostIdAsync(int postId);
}