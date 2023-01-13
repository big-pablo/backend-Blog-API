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
        private IInnerService _innerService;
        public PostController(IPostService postService, IInnerService innerService)
        {
            _postService = postService;
            _innerService = innerService;
        }
        [HttpGet]
        public async Task<ActionResult<List<PostDTO>>> GetPosts([FromQuery] List<string> tags, [FromQuery] string author, [FromQuery] int min, [FromQuery] int max, [FromQuery] PostSortingEnum sorting, [FromQuery] int page, [FromQuery] int size)
        {
            return Ok(await _postService.GetPage(tags, author, min, max, sorting));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<PostFullDTO>> GetCertainPost(string id)
        {
            return Ok(await _postService.GetCertainPost(id));
        }
        [Authorize]
        [HttpPost("{postId}/like")]
        public async Task<ActionResult> AddLike(string postId)
        {
            if (await _innerService.TokenIsInBlackList(HttpContext.Request.Headers)) return Unauthorized("The user is not authorized");
            await _postService.AddLike(postId, await _innerService.GetUserId(HttpContext.User));
            return Ok();
        }
        [Authorize]
        [HttpDelete("{postId}/like")]
        public async Task<ActionResult> RemoveLike(string postId)
        {
            if (await _innerService.TokenIsInBlackList(HttpContext.Request.Headers)) return Unauthorized("The user is not authorized");
            await _postService.RemoveLike(postId, await _innerService.GetUserId(HttpContext.User));
            return Ok();
        }
    }
}
