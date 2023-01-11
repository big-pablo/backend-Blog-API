using Blog.API.Models.Enums;
using System.ComponentModel.DataAnnotations;
namespace Blog.API.Models.DTOs
{
    public class CommentDTO
    {
        [Required]
        public string Id { get; set; }
        [MinLength(1)]
        public string Content { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        [Required]
        public string AuthorId { get; set; }
        [Required]
        [MinLength(1)]
        public string Author { get; set; }
        [Required]
        public int SubComments { get; set; }

    }
}
