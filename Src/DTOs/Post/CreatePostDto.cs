using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Post;

public class CreatePostDto
{
    [Required]
    public string Title { get; set; } = null!;
    [Required]
    public IFormFile? Photo { get; set; }
    [Required]
    public string Description { get; set; } = null!;
}