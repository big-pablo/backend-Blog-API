namespace Blog.API.Models.Entities
{
    public class CommentEntity
    {
        public string Id { get; set; }
        public PostEntity Post { get; set; }
        public UserEntity User { get; set; }
        public CommentEntity ParentComment { get; set; }

    }
}
