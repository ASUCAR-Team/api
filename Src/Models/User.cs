namespace api.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateOnly Birthdate { get; set; }
    public byte[] PasswordHash { get; set; } = null!;
    public byte[] PasswordSalt { get; set; } = null!;
    
    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;

    public List<Skill> Skills { get; set; } = new();
    public List<Post> Posts { get; set; } = new();
    
    public List<Comment> Comments { get; set; } = new();
}