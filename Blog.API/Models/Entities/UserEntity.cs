using Blog.API.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Blog.API.Models.Entities
{
    public class UserEntity
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public GenderEnum Gender { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<PostEntity> LikedPosts { get; set; }
        public List<CommentEntity> CommentedPosts { get; set; }

    }
}
