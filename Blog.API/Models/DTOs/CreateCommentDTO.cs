using System.ComponentModel.DataAnnotations;

namespace Blog.API.Models.DTOs
{
    public class CreateCommentDTO
    {
        [Required]
        [MinLength(1)]
        public string Content { get; set; }
        public string ParentId { get; set; }
    }
}
