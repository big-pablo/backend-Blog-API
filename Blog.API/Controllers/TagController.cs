using Blog.API.Models.Entities;
using Blog.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    [Route("api/tag")]
    [ApiController]
    public class TagController:ControllerBase
    {
        private ITagService _tagService;
        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }
        [HttpGet]
        public async Task<ActionResult<List<TagEntity>>> GetTags()
        {
            try
            {
                return Ok(await _tagService.GetTags());
            }
            catch (Exception exception)
            {
                return StatusCode(500, exception.Message);
            }
        }
    }
}
