namespace Blog.API.Models.Entities
{
    public class LikeEntity
    {
        public string Id { get; set; }
        public PostEntity Post { get; set; }
        public UserEntity User { get; set; }
    }
}
