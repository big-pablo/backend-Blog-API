using Blog.API.Models.DTOs;
using Blog.API.Models.Enums;
using Blog.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    [Route("/api/post")]
    [ApiController]
    public class PostController:ControllerBase
    {
        private IPostService _postService;
        public PostController(IPostService postService)
        {
            _postService = postService;
        }
        [HttpGet]
        public async Task<ActionResult<List<PostDTO>>> GetPosts([FromQuery] List<string> tags, [FromQuery] string author, [FromQuery] int min, [FromQuery] int max, [FromQuery] PostSortingEnum sorting, [FromQuery] int page, [FromQuery] int size)
        {
            return Ok(_postService.GetPage(tags, author, min, max, sorting));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<PostFullDTO>> GetCertainPost(string id)
        {
            return Ok(_postService.GetCertainPost(id));
        }
        [Authorize]
        [HttpPost("{postId}/like")]
        public async Task<ActionResult> AddLike(string postId, [FromQuery] string userId)
        {
            _postService.AddLike(postId, userId); //Переделать на нормальное подбирание id юзера
            return Ok();
        }
        [Authorize]
        [HttpDelete("{postId}/like")]
        public async Task<ActionResult> RemoveLike(string postId, [FromQuery] string userId)
        {
            _postService.RemoveLike(postId, userId);   //Переделать на нормальное подбирание id юзера
            return Ok();
        }
    }
}
