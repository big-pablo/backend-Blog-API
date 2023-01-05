using System.ComponentModel.DataAnnotations;

namespace Blog.API.Models.DTOs
{
    public class TokenResponseDTO
    {
        [Required]
        [MinLength(1)]
        public string Token { get; set; }
    }
}
