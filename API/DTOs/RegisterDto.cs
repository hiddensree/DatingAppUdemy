using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDto
{
    [Required]
    [MaxLength(100)]
    public  string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(8, MinimumLength = 4)]
    public  string Password { get; set; } =  string.Empty;
    public string KnownAs { get; internal set; } = string.Empty;
    public string Gender { get; internal set; } = string.Empty;
    public string City { get; internal set; } = string.Empty;
    public string Country { get; internal set; } = string.Empty;

}