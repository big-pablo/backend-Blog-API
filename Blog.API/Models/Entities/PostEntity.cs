namespace Blog.API.Models.Entities
{
    public class PostEntity
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ReadingTime { get; set; }
        public string? Image { get; set; }
        public UserEntity Author { get; set; }
        public List<TagEntity> Tags { get; set; }

    }
}
