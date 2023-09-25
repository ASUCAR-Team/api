namespace api.DTOs.Post;

public class PostDto
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string PhotoUrl { get; set; } = null!;
    public int TotalLikes { get; set; }
    public int TotalComments { get; set; }
}