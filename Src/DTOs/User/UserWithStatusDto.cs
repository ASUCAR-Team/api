using api.DTOs.Role;

namespace api.DTOs.User;

public class UserWithStatusDto
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public int Age { get; set; }
    public bool IsEnabled { get; set; }
    public RoleDto Role { get; set; } = null!;
}