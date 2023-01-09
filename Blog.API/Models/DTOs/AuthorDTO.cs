using Blog.API.Models.Enums;
using System.ComponentModel.DataAnnotations;
namespace Blog.API.Models.DTOs
{
    public class AuthorDTO
    {
        [Required]
        [MinLength(1)]
        public string FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        [Required]
        public GenderEnum Gender { get; set; }
        public int Posts { get; set; }
        public int Likes { get; set; }
        public DateTime? Created { get; set; }
    }
}
