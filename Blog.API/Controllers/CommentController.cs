using Blog.API.Exceptions;
using Blog.API.Models.DTOs;
using Blog.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    [Route("/api/comment")]
    [ApiController]
    public class CommentController:ControllerBase
    {
        private ICommentService _commentService;
        private IInnerService _innerService;
        public CommentController(ICommentService commentService, IInnerService innerService)
        {
            _commentService = commentService;
            _innerService = innerService;
        }
        [HttpGet("{id}/tree")]
        public async Task<ActionResult<List<CommentDTO>>> GetNestedComments(string id)
        {
            try
            {
                return Ok(_commentService.GetNestedComments(id));
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
        [HttpPost("{id}/comment")]
        [Authorize]
        public async Task<ActionResult> AddComment(string id, CreateCommentDTO commentDTO)
        {
            if (await _innerService.TokenIsInBlackList(HttpContext.Request.Headers)) return Unauthorized("The user is not authorized");
            try
            {
                await _commentService.AddComment(id, commentDTO.ParentId, commentDTO.Content, await _innerService.GetUserId(HttpContext.User));
                return Ok();
            }
            catch (NotFoundException exception)
            {
                return NotFound(exception.Message);
            }
            catch (ValidationException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (Exception exception)
            {
                return StatusCode(500, exception.Message);
            }
        }
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> EditComment(string id,[FromBody] EditCommentDTO content)
        {
            if (await _innerService.TokenIsInBlackList(HttpContext.Request.Headers)) return Unauthorized("The user is not authorized");
            try
            {
                await _commentService.EditComment(id, content.Content, await _innerService.GetUserId(HttpContext.User));
                return Ok();
            }
            catch (ForbiddenException exception)
            {
                return Forbid(exception.Message);
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
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteComment(string id)
        {
            if (await _innerService.TokenIsInBlackList(HttpContext.Request.Headers)) return Unauthorized("The user is not authorized");
            try
            {
                await _commentService.DeleteComment(id, await _innerService.GetUserId(HttpContext.User));
                return Ok();
            }
            catch (ForbiddenException exception)
            {
                return Forbid(exception.Message);
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
