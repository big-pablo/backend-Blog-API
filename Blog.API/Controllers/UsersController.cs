using Blog.API.Models.DTOs;
using Blog.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IAuthService _authService;
        private IUserService _userService;
        private IInnerService _innerService;
        public UsersController(IAuthService authInterface, IUserService userService, IInnerService innerService)
        {
            _authService = authInterface;
            _userService = userService;
            _innerService = innerService;
        }
        [HttpPost("register")]
        public async Task<ActionResult<TokenResponseDTO>> Register(UserRegisterDTO model)
        {
            try
            {
                return await _authService.Register(model);
            }
            catch
            {
                return BadRequest("This mail is already in use");
            }
        }
        [HttpPost("login")]

        public async Task<ActionResult<TokenResponseDTO>> Login(LoginCredentialsDTO model)
        {
            try
            {
                return await _authService.Login(model);
            }
            catch
            {
                return BadRequest("Invalid username or password");
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            if (await _innerService.TokenIsInBlackList(HttpContext.Request.Headers)) return Unauthorized("The user is not authorized");
            var token = await _innerService.GetToken(HttpContext.Request.Headers);
            try
            {
                await _authService.Logout(token);
                return Ok();
            }
            catch
            {
                return BadRequest("The user is not authorized");
            }
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<ActionResult<UserDTO>> GetUserData()
        {
            if (await _innerService.TokenIsInBlackList(HttpContext.Request.Headers)) return Unauthorized("The user is not authorized");
            var response = await _userService.GetUserProfile(await _innerService.GetUserId(HttpContext.User));
            return Ok(response);
        }
        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateUserData(UserEditDTO model)
        {
            if (await _innerService.TokenIsInBlackList(HttpContext.Request.Headers)) return Unauthorized("The user is not authorized");
                await _userService.UpdateUserProfile(await _innerService.GetUserId(HttpContext.User), model);
            return Ok();
        }
    }
}
