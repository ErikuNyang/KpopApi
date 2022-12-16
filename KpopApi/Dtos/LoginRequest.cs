using System.ComponentModel.DataAnnotations;

namespace KpopApi.Dtos;
public class LoginRequest
{
    [Required(ErrorMessage = "User Name is required.")]
    public string UserName { get; set; } = null!;
    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = null!;
}