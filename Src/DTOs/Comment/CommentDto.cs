namespace api.DTOs.Comment;

public class CommentDto
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Body { get; set; } = null!;
}