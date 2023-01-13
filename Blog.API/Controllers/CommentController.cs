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
            return Ok(_commentService.GetNestedComments(id));
        }
        [HttpPost("{id}/comment")]
        [Authorize]
        public async Task<ActionResult> AddComment(string id, CreateCommentDTO commentDTO)
        {
            if (await _innerService.TokenIsInBlackList(HttpContext.Request.Headers)) return Unauthorized("The user is not authorized");
            return Ok(_commentService.AddComment(id, commentDTO.ParentId, commentDTO.Content, await _innerService.GetUserId(HttpContext.User)));
        }
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> EditComment(string commentId, string content)
        {
            if (await _innerService.TokenIsInBlackList(HttpContext.Request.Headers)) return Unauthorized("The user is not authorized");
            return Ok(_commentService.EditComment(commentId, content, await _innerService.GetUserId(HttpContext.User)));
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteComment(string commentId)
        {
            if (await _innerService.TokenIsInBlackList(HttpContext.Request.Headers)) return Unauthorized("The user is not authorized");
            return Ok(_commentService.DeleteComment(commentId, await _innerService.GetUserId(HttpContext.User)));
        }
    }
}
