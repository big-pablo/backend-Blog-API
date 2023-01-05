using System.ComponentModel.DataAnnotations;

namespace Blog.API.Models.DTOs
{
    public class UpdateCommentDTO
    {
        [Required]
        [MinLength(1)]
        public string Content { get; set; }
    }
}
