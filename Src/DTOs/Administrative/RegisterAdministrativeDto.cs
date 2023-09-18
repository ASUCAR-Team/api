using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Administrative;

public class RegisterAdministrativeDto
{
    [Required]
    public string Username { get; set; } = null!;
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string LastName { get; set; } = null!;
    [Required]
    public DateOnly Birthdate { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
}