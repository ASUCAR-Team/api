using System.ComponentModel.DataAnnotations;

namespace api.DTOs.User;

public class UpdatePasswordDto
{
    [Required]
    public string CurrentPassword { get; set; } = null!;
    [Required]
    public string NewPassword { get; set; } = null!;
    [Required]
    [Compare("NewPassword")]
    public string ConfirmNewPassword { get; set; } = null!;
}