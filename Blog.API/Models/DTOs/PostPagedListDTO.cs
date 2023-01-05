namespace Blog.API.Models.DTOs
{
    public class PostPagedListDTO
    {
        public List<PostDTO> Posts { get; set; }
        public PageInfoDTO Pagination { get; set; }

    }
}
