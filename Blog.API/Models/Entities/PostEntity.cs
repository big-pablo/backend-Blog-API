namespace Blog.API.Models.Entities
{
    public class PostEntity
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ReadingTime { get; set; }
        public string Image { get; set; }
        public UserEntity User { get; set; }
        public List<UserEntity> UserLikes { get; set; }
        public List<TagEntity> Tags { get; set; }

    }
}
