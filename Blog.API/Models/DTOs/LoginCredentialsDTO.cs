using System.ComponentModel.DataAnnotations;

namespace Blog.API.Models.DTOs
{
    public class LoginCredentialsDTO
    {
        [Required]
        [MinLength(1)]
        public string Email { get; set; }
        [Required]
        [MinLength(1)]
        public string Password { get; set; }
    }
}
