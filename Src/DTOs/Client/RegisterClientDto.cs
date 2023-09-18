using System.ComponentModel.DataAnnotations;

namespace api.DTOs.User;

public class RegisterClientDto
{
    [Required]
    public string Username { get; set; } = null!;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string LastName { get; set; } = null!;
    [Required]
    public DateOnly Birthdate { get; set; }
    [Required]
    public string Password { get; set; } = null!;
    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; } = null!;
}