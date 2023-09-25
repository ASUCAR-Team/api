using api.DTOs.Comment;
using api.DTOs.Post;
using api.Extensions;
using api.Models;
using api.Repositories.Interfaces;
using api.Services.Interfaces;
using api.Src.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Authorize]
public class PostController : BaseApiController
{
    private readonly ICommentRepository _commentRepository;
    private readonly ILikeRepository _likeRepository;
    private readonly IPhotoService _photoService;
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;

    public PostController(
        ICommentRepository commentRepository,
        ILikeRepository likeRepository,
        IPhotoService photoService,
        IPostRepository postRepository,
        IUserRepository userRepository)
    {
        _commentRepository = commentRepository;
        _likeRepository = likeRepository;
        _photoService = photoService;
        _postRepository = postRepository;
        _userRepository = userRepository;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostDto>>> GetPosts()
    {
        try
        {
            var user = await _userRepository.GetUserByIdAsync(User.GetUserId());

            if (user is not null && user.DisabledAt != DateTime.MinValue)
                return BadRequest();
        }
        catch (Exception) { }
        
        var posts = await _postRepository.GetPostsAsync();

        return Ok(posts);
    }

    [HttpPost]
    public async Task<ActionResult> CreatePost(
        [FromForm] CreatePostDto createPostDto)
    {
        var user = await _userRepository.GetUserByIdAsync(User.GetUserId());
        
        if (user is null || user.DisabledAt != DateTime.MinValue || createPostDto.Photo is null)
            return BadRequest();
        
        var result = await _photoService.AddPhotoAsync(createPostDto.Photo);

        if (result.Error != null)
            return BadRequest(result.Error.Message);

        var post = new Post
        {
            Title = createPostDto.Title,
            Description = createPostDto.Description,
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId,
            User = user
        };

        user.Posts.Add(post);

        if (!await _userRepository.SaveAllAsync())
            return BadRequest();

        return Ok();
    }

    [HttpPost("{postId}/react")]
    public async Task<ActionResult> ReactPost(
        [FromRoute] int postId)
    {
        var user = await _userRepository.GetUserByIdAsync(User.GetUserId());

        if (user is null || user.DisabledAt != DateTime.MinValue)
            return BadRequest();

        var post = await _postRepository.GetPostByIdAsync(postId);

        if (post is null || post.Likes.Any(l => l.UserId == user.Id))
            return BadRequest();
        
        var like = new Like
        {
            User = user,
            Post = post
        };

        post.Likes.Add(like);

        if (!await _userRepository.SaveAllAsync())
            return BadRequest();
        
        return Ok();
    }

    [HttpPost("{postId}/comment")]
    public async Task<ActionResult> CommentPost(
        [FromRoute] int postId,
        [FromBody] CreateCommentDto commentDto)
    {
        var user = await _userRepository.GetUserByIdAsync(User.GetUserId());

        if (user is null || user.DisabledAt != DateTime.MinValue)
            return BadRequest();

        var post = await _postRepository.GetPostByIdAsync(postId);

        if (post is null)
            return BadRequest();
        
        var comment = new Comment
        {
            Body = commentDto.Body,
            User = user,
            Post = post
        };
        
        post.Comments.Add(comment);

        if (!await _userRepository.SaveAllAsync())
            return BadRequest();
        
        return Ok();
    }

    [HttpGet("{postId}/comments")]
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments(
        [FromRoute] int postId)
    {
        var user = await _userRepository.GetUserByIdAsync(User.GetUserId());

        if (user is null || user.DisabledAt != DateTime.MinValue)
            return BadRequest();

        var post = await _postRepository.GetPostByIdAsync(postId);

        if (post is null)
            return BadRequest();
        
        var comments = await _commentRepository.GetCommentsByPostIdAsync(postId);

        return Ok(comments);
    }

    [HttpDelete("{postId}/remove-react")]
    public async Task<ActionResult> RemoveReactPost(
        [FromRoute] int postId)
    {
        var user = await _userRepository.GetUserByIdAsync(User.GetUserId());

        if (user is null || user.DisabledAt != DateTime.MinValue)
            return BadRequest();

        var like = await _likeRepository.GetLikeByPostIdAndUserIdAsync(postId, user.Id);
        var post = await _postRepository.GetPostByIdAsync(postId);

        if (like is null || post is null)
            return BadRequest();
        
        post.Likes.Remove(like);

        if (!await _userRepository.SaveAllAsync())
            return BadRequest();
        
        return Ok();
    }
}