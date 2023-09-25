namespace api.Models;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Url { get; set; } = null!;
    public string PublicId { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public List<Comment> Comments { get; set; } = new();
    public List<Like> Likes { get; set; } = new();
}