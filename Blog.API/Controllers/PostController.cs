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
        //[Authorize]
        [HttpGet]
        public async Task<ActionResult<List<PostDTO>>> GetPosts([FromQuery] List<string>? tags, string? author, [FromQuery] int? min, [FromQuery] int? max, [FromQuery] PostSortingEnum sorting, [FromQuery] int page=1, [FromQuery] int size = 6)
        {
            //if (await _innerService.TokenIsInBlackList(HttpContext.Request.Headers)) return Unauthorized("The user is not authorized");
            return Ok(await _postService.GetPage(author, sorting, page, size, min, max, tags));
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<PostFullDTO>> GetCertainPost(string id)
        {
            if (await _innerService.TokenIsInBlackList(HttpContext.Request.Headers)) return Unauthorized("The user is not authorized");
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
