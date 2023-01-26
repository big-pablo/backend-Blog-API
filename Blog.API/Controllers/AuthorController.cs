using Blog.API.Models.DTOs;
using Blog.API.Models.Entities;
using Blog.API.Services;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    [Route("api/author/list")]
    [ApiController]
    public class AuthorController:ControllerBase
    {
        private IAuthorService _authorService;
        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }
        [HttpGet]
        public async Task<ActionResult<List<AuthorDTO>>> GetAuthors()
        {
            try
            {
                return (await _authorService.GetAuthors());
            }
            catch (Exception exception)
            {
                return StatusCode(500, exception.Message);
            }
        }
    }
}
