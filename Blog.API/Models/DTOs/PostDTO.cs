using System.ComponentModel.DataAnnotations;

namespace Blog.API.Models.DTOs
{
    public class PostDTO
    {
        [Required]
        public string Id { get; set; }
        [Required]
        [MinLength(1)]
        public string Title { get; set; }
        [Required]
        [MinLength(1)]
        public string Description { get; set; }
        [Required]
        public int ReadingTime { get; set; }
        public string Image { get; set; }
        [Required]
        public string AuthorId { get; set; }
        [Required]
        [MinLength(1)]
        public string Author { get; set; }
        [Required]
        public int Likes { get; set; }
        [Required]
        public bool HasLike { get; set; }
        [Required]
        public int CommentsCount { get; set; }
        public List<TagDTO> Tags { get; set; }
    }
}
