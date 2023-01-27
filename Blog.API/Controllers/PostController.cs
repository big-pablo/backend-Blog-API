using Blog.API.Exceptions;
using Blog.API.Models;
using Blog.API.Models.DTOs;
using Blog.API.Models.Enums;
using Blog.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

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
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<PostDTO>>> GetPosts([FromQuery] List<string>? tags, string? author, [FromQuery] int? min, [FromQuery] int? max, [FromQuery] PostSortingEnum sorting, [FromQuery] int page=1, [FromQuery] int size = 6)
        {
            string id;
            try
            {
                id = await _innerService.GetUserId(HttpContext.User);
            }
            catch
            {
                id = "";
            }
            try
            {
                return Ok(await _postService.GetPage(id, author, sorting, page, size, min, max, tags));
            }
            catch (NotFoundException exception)
            {
                return NotFound(exception.Message);
            }
            catch (Exception exception)
            {
                return StatusCode(500, exception.Message);
            }   
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<PostFullDTO>> GetCertainPost(string id)
        {
            try
            {
                return Ok(await _postService.GetCertainPost(id));
            }
            catch (NotFoundException exception)
            {
                return NotFound(exception.Message);
            }
            catch (Exception exception)
            {
                return StatusCode(500, exception.Message);
            }
        }
        [Authorize]
        [HttpPost("{postId}/like")]
        public async Task<ActionResult> AddLike(string postId)
        {
            if (await _innerService.TokenIsInBlackList(HttpContext.Request.Headers)) return Unauthorized("The user is not authorized");
            try
            {
                await _postService.AddLike(postId, await _innerService.GetUserId(HttpContext.User));
                return Ok();
            }
            catch (NotFoundException exception)
            {
                return NotFound(exception.Message);
            }
            catch (ObjectExistsException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (Exception exception)
            {
                return StatusCode(500, exception.Message);
            }
        }
        [Authorize]
        [HttpDelete("{postId}/like")]
        public async Task<ActionResult> RemoveLike(string postId)
        {
            if (await _innerService.TokenIsInBlackList(HttpContext.Request.Headers)) return Unauthorized("The user is not authorized");
            try
            {
                await _postService.RemoveLike(postId, await _innerService.GetUserId(HttpContext.User));
                return Ok();
            }
            catch (NotFoundException exception)
            {
                return NotFound(exception.Message);
            }
            catch (Exception exception)
            {
                return StatusCode(500, exception.Message);
            }
        }
    }
}
