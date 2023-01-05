using System.ComponentModel.DataAnnotations;

namespace Blog.API.Models.DTOs
{
    public class TagDTO
    {
        [Required]
        public string Id { get; set; }
        [Required]
        [MinLength(1)]
        public string Name { get; set; }
    }
}
