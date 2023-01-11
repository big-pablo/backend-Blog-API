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
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }
        [HttpGet("{id}/tree")]
        public async Task<ActionResult<List<CommentDTO>>> GetNestedComments(string id)
        {
            return Ok(_commentService.GetNestedComments(id));
        }
        [HttpPost("{id}/comment")]
        [Authorize]
        public async Task<ActionResult> AddComment(string id, CreateCommentDTO commentDTO, string userId)
        {
            return Ok(_commentService.AddComment(id, commentDTO.ParentId, commentDTO.Content, userId));
        }
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> EditComment(string commentId, string content)
        {
            return Ok(_commentService.EditComment(commentId, content));
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteComment(string commentId)
        {
            return Ok(_commentService.DeleteComment(commentId));
        }
    }
}
